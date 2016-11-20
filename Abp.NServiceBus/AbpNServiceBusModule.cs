using Abp.Dependency;
using Abp.Modules;
using Abp.Runtime.Session;
using Castle.MicroKernel.Registration;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.MessageMutator;
using NServiceBus.Persistence;
using NServiceBus.Transport.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus
{
    public class AbpNServiceBusModule : AbpModule
    {
        /// <summary>
        /// Indicate if Abp.NServiceBus should use custom IAbpSession (default: true)
        /// </summary>
        public bool UseAbpNServiceBusSession { get; set; }

        public AbpNServiceBusModule()
        {
            UseAbpNServiceBusSession = true;
        }

        public override void PreInitialize()
        {
            // Module Config
            IocManager.Register<AbpNServiceBusModuleConfig>(DependencyLifeStyle.Singleton);

            if (UseAbpNServiceBusSession)
            {
                // Replace IAbpSession
                Configuration.ReplaceService(typeof(IAbpSession), () =>
                {
                    IocManager.IocContainer.Register(
                        Component.For<IAbpSession>()
                                 .ImplementedBy<AbpNServiceBusSession>()
                                 .IsDefault()
                                 .LifeStyle.Is(Castle.Core.LifestyleType.Scoped)
                    );
                });

                // Default IsolationLevel
                Configuration.UnitOfWork.IsTransactional = false;
                Configuration.UnitOfWork.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            // Create NServiceBus Endpoint Configuration
            var config = Configuration.Modules.NServiceBus();
            var endpointConfiguration = Configuration.Modules.NServiceBus().EndpointConfiguration = new EndpointConfiguration(config.EndpointName);

            // Configure Logging
            if (!string.IsNullOrEmpty(config.LogDirectory))
            {
                var defaultFactory = LogManager.Use<DefaultFactory>();
                var logDirPath = config.LogDirectory;
                defaultFactory.Directory(logDirPath);
                if (config.LogLevel != null)
                    defaultFactory.Level(config.LogLevel.Value);

            }

            // Endpoint Level Config
            endpointConfiguration.PurgeOnStartup(false);
            endpointConfiguration.SendFailedMessagesTo(config.ErrorQueue);
            endpointConfiguration.AuditProcessedMessagesTo(config.AuditQueue);

            // Transport
            endpointConfiguration.ConfigureAbpNServiceBusDefaultTransport();

            // MaxConcurrencyLevel
            if (config.MaximumConcurrencyLevel.HasValue)
                endpointConfiguration.LimitMessageProcessingConcurrencyTo(config.MaximumConcurrencyLevel.Value);

            // Persistence
            endpointConfiguration.ConfigureAbpNServiceBusDefaultPersistence();

            // Outbox
            if(config.UseOutbox)
                endpointConfiguration.EnableOutbox();

            // Unobtrusive Message Mode
            endpointConfiguration.UseAbpNServiceBusMessageConventions();

            // Container
            endpointConfiguration.UseContainer<WindsorBuilder>(
                customizations: customizations =>
                {
                    customizations.ExistingContainer(IocManager.IocContainer);
                });

            // Recoverability
            endpointConfiguration.Recoverability()
                .Immediate(customizations: immediate =>
                {
                    immediate.NumberOfRetries(config.ImmediateRetries);
                })
                .Delayed(customizations: delayed =>
                {
                    delayed.NumberOfRetries(config.DelayedRetries);
                    delayed.TimeIncrease(TimeSpan.FromSeconds(config.DelayedRetriesTimeIncreaseInSeconds));
                });

            // AbpSession session Mutator to propagate Headers to all output messages
            endpointConfiguration.RegisterComponents(
                registration: components =>
                {
                    components.ConfigureComponent<AbpNServiceBusSessionHeaderAppender>(DependencyLifecycle.InstancePerCall);
                });

            // UnitOfWork
            endpointConfiguration.Pipeline.Register(typeof(AbpNServiceBusUnitOfWork), typeof(AbpNServiceBusUnitOfWork).Name);
        }

        public override void PostInitialize()
        {
            var config = Configuration.Modules.NServiceBus();
            var endpointConfiguration = config.EndpointConfiguration;

            // Start NServiceBus Endpoint
            Logger.InfoFormat(string.Format("NServiceBus endpoint {0} is starting...", config.EndpointName));

            endpointConfiguration.EnableInstallers();
            var endpointInstance = Endpoint.Start(endpointConfiguration)
                                           .GetAwaiter()
                                           .GetResult();

            // Register Endpoint Instance into Container
            Logger.DebugFormat("Registering IEndpointInstance into container");
            IocManager.IocContainer.Register(
                Component.For<IEndpointInstance>().Instance(endpointInstance)
            );

            Logger.InfoFormat(string.Format("NServiceBus endpoint {0} started successfully", config.EndpointName));
        }

        public override void Shutdown()
        {
            // Stop NServiceBus Endpoint
            var config = Configuration.Modules.NServiceBus();

            IEndpointInstance endpoint = IocManager.Resolve<IEndpointInstance>();
            endpoint.Stop()
                    .GetAwaiter()
                    .GetResult();

            Logger.InfoFormat(string.Format("NServiceBus endpoint {0} shutdown successfully", config.EndpointName));
        }
    }
}
