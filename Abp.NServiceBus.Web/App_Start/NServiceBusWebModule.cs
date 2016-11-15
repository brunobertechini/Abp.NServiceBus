using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Modules;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using Abp.Zero.Configuration;
using Abp.NServiceBus.Api;
using Hangfire;
using NServiceBus;
using System;
using NServiceBus.Logging;
using System.Web;
using Castle.MicroKernel.Registration;
using NServiceBus.Transport.SQLServer;
using NServiceBus.Persistence;
using System.Configuration;

namespace Abp.NServiceBus.Web
{
    [DependsOn(
        typeof(NServiceBusDataModule),
        typeof(NServiceBusApplicationModule),
        typeof(NServiceBusWebApiModule),
        typeof(AbpWebSignalRModule),
        //typeof(AbpHangfireModule), - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
        typeof(AbpWebMvcModule),
        typeof(AbpNServiceBusModule)
    )]
    public class NServiceBusWebModule : AbpModule
    {
        public const string EndpointName = "Abp.NServiceBus.Web";

        public NServiceBusWebModule(AbpNServiceBusModule nsbModule)
        {
            nsbModule.UseAbpNServiceBusSession = false;
        }

        public override void PreInitialize()
        {
            //Enable database based localization
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<NServiceBusNavigationProvider>();

            //Configure Hangfire - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
            //Configuration.BackgroundJobs.UseHangfire(configuration =>
            //{
            //    configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            //});
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        public override void PostInitialize()
        {
            Console.WriteLine(string.Format("Starting NServiceBus Endpoint {0}...", EndpointName));

            var defaultFactory = LogManager.Use<DefaultFactory>();
            defaultFactory.Directory(HttpContext.Current.Server.MapPath("~/App_Data/Logs/"));

            var endpointConfiguration = new EndpointConfiguration(EndpointName);
            endpointConfiguration.PurgeOnStartup(false);
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            // Default Connection String
            var defaultConnString = ConfigurationManager.ConnectionStrings[NServiceBusConsts.ConnectionStringName].ConnectionString;

            // SQL Server Transport - (Package NServiceBus.SqlServer)
            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.ConnectionString(defaultConnString);
            transport.DefaultSchema(NServiceBusConsts.EndpointDatabaseSchema);

            // NHibernate Persistence - (Package NServiceBus.NHibernate)
            var nhConfiguration = new NHibernate.Cfg.Configuration
            {
                Properties =
                {
                    ["dialect"] = "NHibernate.Dialect.MsSql2012Dialect",
                    ["connection.provider"] = "NHibernate.Connection.DriverConnectionProvider",
                    ["connection.driver_class"] = "NHibernate.Driver.SqlClientDriver",
                    ["connection.default_schema"] = NServiceBusConsts.EndpointDatabaseSchema,
                    ["connection.connection_string"] = defaultConnString
                }
            };
            var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();
            persistence.UseConfiguration(nhConfiguration);

            // Unobtrusive Message Mode
            endpointConfiguration.UseAbpNServiceBusMessageConventions();

            // Disable Immediate & Delayed Retries 
            endpointConfiguration.Recoverability()
                .Immediate(customizations: immediate =>
                {
                    immediate.NumberOfRetries(0);
                })
                .Delayed(customizations: delayed =>
                {
                    delayed.NumberOfRetries(0);
                    delayed.TimeIncrease(TimeSpan.FromSeconds(30));
                });

            // Configure NServiceBus with Abp IocContainer - (Package NServiceBus.CastleWindsor)
            endpointConfiguration.UseContainer<WindsorBuilder>(
                customizations: customizations =>
                {
                    customizations.ExistingContainer(IocManager.IocContainer);
                });

            // Enable Installers & Start Endpoint
            endpointConfiguration.EnableInstallers();
            var endpointInstance = Endpoint.Start(endpointConfiguration)
                                           .GetAwaiter()
                                           .GetResult();

            // Register Endpoint Instance to Abp Container
            IocManager.IocContainer.Register(
                Component.For<IEndpointInstance>().Instance(endpointInstance)
            );

            Console.WriteLine(string.Format("NServiceBus endpoint {0} started successfully", EndpointName));
        }

        public override void Shutdown()
        {
            IEndpointInstance endpoint = IocManager.Resolve<IEndpointInstance>();
            endpoint.Stop()
                    .GetAwaiter()
                    .GetResult();

            Console.WriteLine(string.Format("NServiceBus endpoint {0} shutdown successfully", EndpointName));
        }
    }
}
