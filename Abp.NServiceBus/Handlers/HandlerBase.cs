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

        protected IAbpSession AbpSession { get; private set; }

        protected IUnitOfWorkManager UowManager { get; private set; }

        public HandlerBase(IAbpSession session, IUnitOfWorkManager uowManager)
        {
            AbpSession = session;
            UowManager = uowManager;
        }

        protected void LogContextInfo(IMessageHandlerContext context)
        {
            Logger.DebugFormat("Message: {0}", context.MessageId);
            Logger.DebugFormat("Message/AbpSession: {0}/{1}", context.MessageId, AbpSession.GetHashCode());
            Logger.DebugFormat("Message/UowManager: {0}/{1}", context.MessageId, UowManager.GetHashCode());

            if (UowManager.Current != null)
                Logger.DebugFormat("Message/UnitOfWork: {0}/{1}", context.MessageId, UowManager.Current.GetHashCode());
        }
    }
}
