using Abp.Modules;
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
