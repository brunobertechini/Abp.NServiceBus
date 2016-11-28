using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Castle.Core.Logging;
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
        protected ILog EndpointLogger = LogManager.GetLogger<T>();

        public ILogger BusinessLogger { get; set; }

        public IAbpSession AbpSession { get; set; }

        public IUnitOfWorkManager UowManager { get; set; }

        public AbpNServiceBusModuleConfig AbpNServiceBusModuleConfig { get; set; }

        public HandlerBase()
        {
            BusinessLogger = NullLogger.Instance;
        }

        protected void LogContextInfo(IMessageHandlerContext context)
        {
            if (AbpNServiceBusModuleConfig.Debug)
            {
                EndpointLogger.DebugFormat("Message: {0}", context.MessageId);
                EndpointLogger.DebugFormat("Message/AbpSession: {0}/{1}", context.MessageId, AbpSession.GetHashCode());
                EndpointLogger.DebugFormat("Message/UowManager: {0}/{1}", context.MessageId, UowManager.GetHashCode());

                if (UowManager.Current != null)
                    EndpointLogger.DebugFormat("Message/UnitOfWork: {0}/{1}", context.MessageId, UowManager.Current.GetHashCode());
            }
        }
    }
}
