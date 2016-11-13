using Abp.Dependency;
using Abp.Modules;
using Abp.Runtime.Session;
using Castle.MicroKernel.Registration;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Transport.SQLServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus
{
    [DependsOn(
        typeof(NServiceBusCoreModule),
        typeof(NServiceBusApplicationModule),
        typeof(NServiceBusDataModule),
        typeof(AbpNServiceBusModule)
    )]
    public class Endpoint1Bootstrapper : AbpModule
    {
        public const string EndpointName = "Abp.NServiceBus.Endpoint1";

        public override void PreInitialize()
        {
            Configuration.ReplaceService(typeof(IAbpSession), () =>
            {
                IocManager.Register<IAbpSession, AbpNServiceBusSession>(DependencyLifeStyle.Singleton);
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
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
            endpointConfiguration.UseAbpMessageConventions();

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

            // Configure mutator to add AbpSession headers to all output messages
            endpointConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<AbpNServiceBusWebSessionHeaderAppender>(DependencyLifecycle.InstancePerCall);
                });

            // Configure Behavior for AbpNServiceBusSession
            endpointConfiguration.Pipeline.Register(typeof(AbpNServiceBusUnitOfWork), typeof(AbpNServiceBusUnitOfWork).Name);

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
