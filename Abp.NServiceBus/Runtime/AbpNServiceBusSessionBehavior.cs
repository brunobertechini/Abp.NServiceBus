using Abp.Dependency;
using Abp.Runtime.Session;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus.Runtime
{
    public class AbpNServiceBusSessionBehavior : Behavior<IIncomingPhysicalMessageContext>
    {
        private ILog Logger = LogManager.GetLogger<AbpNServiceBusUnitOfWork>();

        public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            Logger.DebugFormat("Message: {0}", context.MessageId);

            // Get AbpSession
            IAbpSession session = IocManager.Instance.Resolve<IAbpSession>();
            Logger.DebugFormat("Message/AbpSession: {0}/{1}", context.MessageId, session.GetHashCode());

            AbpNServiceBusSession nsbSession = session as AbpNServiceBusSession;
            nsbSession.SetHeaders(context.MessageHeaders);

            return next();
        }
    }
}
