using Abp.Modules;
using Abp.Runtime.Session;
using Castle.MicroKernel.Registration;
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
        public override void PreInitialize()
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

            // Module Config
            IocManager.Register<AbpNServiceBusModuleConfig>();

            // Default IsolationLevel
            //Configuration.UnitOfWork.Scope = System.Transactions.TransactionScopeOption.Required;
            Configuration.UnitOfWork.IsTransactional = false;
            Configuration.UnitOfWork.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;

            // Mutator
            //IocManager.Register<IMutateOutgoingTransportMessages, HeaderPropagationMutator>(DependencyLifeStyle.Transient);

            // TODO Encapsulate default EndpointConfig and allow further customizations
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

    }
}
