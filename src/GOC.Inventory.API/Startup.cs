using Microphone.AspNet;
using Microphone.Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using Serilog;
using System;
using EasyNetQ;
using Microsoft.AspNetCore.Http;
using SimpleInjector.Integration.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using SimpleInjector.Lifestyles;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microphone;
using Microphone.Core;
using GOC.Inventory.API.Adapters;
using GOC.Inventory.API.Interfaces;
using GOC.Inventory.API.EventBus;
using GOC.Inventory.Domain.AggregatesModels.InventoryAggregate;
using GOC.Inventory.Infrastructure.Repositories;
using GOC.Inventory.Infrastructure;
using Microsoft.EntityFrameworkCore;
using GOC.Inventory.Domain.Events;
using GOC.Inventory.Domain;
using GOC.Inventory.Domain.AggregatesModels.VendorAggregate;
using GOC.Inventory.Domain.AggregatesModels.CompanyAggregate;
using GOC.Inventory.API.Application.Interfaces;
using GOC.Inventory.API.Application.Services;
using System.Reflection;
using GOC.Inventory.API.Application.BackgroundServices;
using Microsoft.Extensions.Hosting;

namespace GOC.Inventory.API
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);


            Configuration = builder.Build();
            AppSettings = Configuration.GetSection("InventoryService").Get<AppSettings>();

        }
        public static AppSettings AppSettings { get; set; }
        public IConfiguration Configuration { get; }
        private Container Container { get; } = new Container();
        private ILoggerFactory LoggerFactory { get; set; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //logging
            LoggerFactory = new LoggerFactory();

            services.AddSingleton(LoggerFactory);
            LoggerFactory.AddSerilog();
            LoggerFactory.AddDebug();
            var serilogConfiguration = new LoggerConfiguration().ReadFrom.Configuration(Configuration);
            Log.Logger = serilogConfiguration.CreateLogger().ForContext("Application", "InventoryService");

            //auth
            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            services.AddAuthentication("Bearer")
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = "http://vagrant:5000";
                options.ApiSecret = "api2-secret";
                options.RequireHttpsMetadata = false;
                options.ApiName = "api2";
            });

            //consul
            services.AddMicrophone<ConsulProvider>();

            services.Configure<ConsulOptions>(options =>
            {
                options.Heartbeat = AppSettings.Consul.Heartbeat;
                options.Host = AppSettings.Consul.Host;
                options.Port = AppSettings.Consul.Port;
            });

            //DI
            IntegrateSimpleInjector(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IHttpContextAccessor context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();

            app.UseMvc()
               .UseMicrophone("InventoryService", "1.0", new Uri($"http://vagrant:5002/"));
            
            InitializeContainer(LoggerFactory, context);

            FireupBackgroungTasks();
        }

        private void IntegrateSimpleInjector(IServiceCollection services)
        {
            Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(
            new SimpleInjectorControllerActivator(Container));
            
            services.AddSingleton<IViewComponentActivator>(
                new SimpleInjectorViewComponentActivator(Container));


            services.EnableSimpleInjectorCrossWiring(Container);
            services.UseSimpleInjectorAspNetRequestScoping(Container);
        }

        protected void InitializeContainer(ILoggerFactory logger, IHttpContextAccessor context)
        {
            var hybridLifestyle = Lifestyle.CreateHybrid(
                lifestyleSelector: () => context.HttpContext != null,
                trueLifestyle: Lifestyle.Scoped,
                falseLifestyle: Lifestyle.Singleton);
            
            //repos and service
            Container.Register<IInventoryRepository, InventoryRepository>(Lifestyle.Scoped);
            Container.Register<IItemRepository, ItemRepository>(Lifestyle.Scoped);
            Container.Register<IVendorRepository, VendorRepository>(hybridLifestyle);
            Container.Register<ICompanyRepository, CompanyRepository>(hybridLifestyle);
            Container.Register<IInventoryService, InventoryService>(Lifestyle.Scoped);


            // database context registration
            var options = DbContextOptionsBuilder();
            Container.Register(() => new DatabaseContext(options.Options), hybridLifestyle);


            // message bus regs
            var easyNetQLogger = new EasyNetQLoggingAdapter(logger);
            Container.Register<IAdvancedBus>(() => RabbitHutch.CreateBus($"host={AppSettings.Rabbit.Host};" +
                                                           $"publisherConfirms={AppSettings.Rabbit.PublisherConfirms};" +
                                                           $"timeout={AppSettings.Rabbit.Timeout}", 
                                                           x => x.Register<IEasyNetQLogger>(_ => easyNetQLogger)).Advanced, Lifestyle.Singleton);
            
            Container.Register<IEventPublisher, EventPublisher>(hybridLifestyle);
            Container.Register<IEventConsumer, EventConsumer>(hybridLifestyle);
            Container.Register<IMessageRouter, MessageRouter>(hybridLifestyle);


            // logging regs
            Container.Register(() => logger, Lifestyle.Singleton);
            Container.Register(typeof(ILogger<>), typeof(LoggingAdapter<>));

            //consul regs
            Container.RegisterSingleton<IHealthCheck>(new EmptyHealthCheck());

            //domain events registrations          
            Container.Register(typeof(IHandle<>), new[] { Assembly.GetEntryAssembly() }, Lifestyle.Scoped);
            DomainEvents.Container = Container;

            //hosted services for background tasks
            Container.Register<IHostedService, BackgroundEventSubcriptionService>(hybridLifestyle);

            // verify
            Container.Verify();

        }

        private void FireupBackgroungTasks()
        {
            //// polling task for consuming messages for this service
            var backGroundService = (BackgroundEventSubcriptionService)Container.GetInstance(typeof(IHostedService));

            backGroundService.StartAsync(new System.Threading.CancellationToken());

            //var eventConsumer = (EventConsumer)Container.GetInstance(typeof(IEventConsumer));
            //eventConsumer.Consume(new Queue(AppSettings.Rabbit.ConsumableQueue, false));

        }


        private DbContextOptionsBuilder<DatabaseContext> DbContextOptionsBuilder()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>();
            options.UseNpgsql(AppSettings.PostGres.ConnectionString);
            return options;
        }
    }
}
