using Abp.Dependency;
using Abp.Modules;
using Abp.Runtime.Session;
using Castle.MicroKernel.Registration;
using NServiceBus.MessageMutator;
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
            IocManager.Register<AbpNServiceBusModuleConfig>();

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

            // Mutator
            IocManager.Register<IMutateOutgoingTransportMessages, AbpNServiceBusSessionHeaderAppender>(DependencyLifeStyle.Transient);

            // TODO Encapsulate default EndpointConfig and allow further customizations
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

    }
}
