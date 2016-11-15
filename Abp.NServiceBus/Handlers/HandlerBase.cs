using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus
{
    public abstract class HandlerBase<T>
    {
        protected ILog Logger = LogManager.GetLogger<T>();

        public IAbpSession AbpSession { get; set; }

        public IUnitOfWorkManager UowManager { get; set; }

        public AbpNServiceBusModuleConfig AbpNServiceBusModuleConfig { get; set; }

        protected void LogContextInfo(IMessageHandlerContext context)
        {
            if (AbpNServiceBusModuleConfig.Debug)
            {
                Logger.DebugFormat("Message: {0}", context.MessageId);
                Logger.DebugFormat("Message/AbpSession: {0}/{1}", context.MessageId, AbpSession.GetHashCode());
                Logger.DebugFormat("Message/UowManager: {0}/{1}", context.MessageId, UowManager.GetHashCode());

                if (UowManager.Current != null)
                    Logger.DebugFormat("Message/UnitOfWork: {0}/{1}", context.MessageId, UowManager.Current.GetHashCode());
            }
        }
    }
}
