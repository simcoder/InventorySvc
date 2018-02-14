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
using GOC.Inventory.API.Application.EventHandlers;
using System.Reflection;

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();

            app.UseMvc()
               .UseMicrophone("InventoryService", "1.0", new Uri($"http://vagrant:5002/"));
            
            InitializeContainer(LoggerFactory);

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

        protected void InitializeContainer(ILoggerFactory logger)
        {
            //repos and service
            Container.Register<IInventoryRepository, InventoryRepository>(Lifestyle.Scoped);
            Container.Register<IItemRepository, ItemRepository>(Lifestyle.Scoped);
            Container.Register<IVendorRepository, VendorRepository>(Lifestyle.Scoped);
            Container.Register<ICompanyRepository, CompanyRepository>(Lifestyle.Scoped);
            Container.Register<IInventoryService, InventoryService>(Lifestyle.Scoped);


            // database context registration
            var options = DbContextOptionsBuilder();
            Container.Register(() => new DatabaseContext(options.Options),Lifestyle.Scoped);

            // message bus regs
            var easyNetQLogger = new EasyNetQLoggingAdapter(logger);
            Container.Register<IAdvancedBus>(() => RabbitHutch.CreateBus($"host={AppSettings.Rabbit.Host};" +
                                                           $"publisherConfirms={AppSettings.Rabbit.PublisherConfirms};" +
                                                           $"timeout={AppSettings.Rabbit.Timeout}", 
                                                           x => x.Register<IEasyNetQLogger>(_ => easyNetQLogger)).Advanced, Lifestyle.Singleton);
            
            Container.Register<IEventPublisher, EventPublisher>(Lifestyle.Scoped);

            // logging regs
            Container.Register(() => logger, Lifestyle.Singleton);
            Container.Register(typeof(ILogger<>), typeof(LoggingAdapter<>));

            //consul regs
            Container.RegisterSingleton<IHealthCheck>(new EmptyHealthCheck());

            //domain events registrations          
            Container.Register(typeof(IHandle<>), new[] { Assembly.GetEntryAssembly() }, Lifestyle.Scoped);
            //Container.Register<IHandle<InventoryCreated>, InventoryCreatedEventHandler>(Lifestyle.Scoped);
            DomainEvents.Container = Container;

            // verify
            Container.Verify();

        }

        private DbContextOptionsBuilder<DatabaseContext> DbContextOptionsBuilder()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>();
            options.UseNpgsql(AppSettings.PostGres.ConnectionString);
            return options;
        }
    }
}
