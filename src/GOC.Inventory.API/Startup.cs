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
using System.Reflection;
using System.Linq;
using EasyNetQ;
using GOC.Inventory.Domain.AggregatesModels.ProductAggregate;
using Microsoft.AspNetCore.Http;
using SimpleInjector.Integration.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using SimpleInjector.Lifestyles;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microphone;
using Microphone.Core;
using GOC.Inventory.API.Interfaces;
using GOC.Inventory.API.EventBus;
using GOC.Inventory.API.Adapters;

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

            // auto register all other dependencies
            var repositoryAssembly = Assembly.GetEntryAssembly();
            var registrations = repositoryAssembly.GetExportedTypes()
                                                  .Where(type =>
                                                         type.Namespace == "GOC.ApiGateway.Services" ||
                                                         type.Namespace == "GOC.ApiGateway.Interfaces")
                                                  .Where(type => type.GetInterfaces().Any())
                                                  .Select(type => new { Service = type.GetInterfaces().Single(), Implementation = type });
            foreach (var reg in registrations)
            {
                Container.Register(reg.Service, reg.Implementation, Lifestyle.Scoped);
            }

            //services/repos
            Container.Register<IProductRepository, Product>(Lifestyle.Scoped);

            // message bus regs
            var easyNetQLogger = new EasyNetQLoggingAdapter(logger);
            Container.Register(() => RabbitHutch.CreateBus($"host={AppSettings.Rabbit.Host};" +
                                                           $"publisherConfirms={AppSettings.Rabbit.PublisherConfirms};" +
                                                           $"timeout={AppSettings.Rabbit.Timeout}", 
                                                           x => x.Register<IEasyNetQLogger>(_ => easyNetQLogger)).Advanced, Lifestyle.Singleton);
            Container.Register<IEventPublisher<InventoryServiceMessage>, EventPublisher<InventoryServiceMessage>>(Lifestyle.Singleton);

            // logging regs
            Container.Register(() => logger, Lifestyle.Singleton);
            Container.Register(typeof(ILogger<>), typeof(LoggingAdapter<>));

            //consul regs
            Container.RegisterSingleton<IHealthCheck>(new EmptyHealthCheck());

            // verify
            Container.Verify();

        }
    }
}
