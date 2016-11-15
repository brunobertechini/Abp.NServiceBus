using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using NServiceBus.Logging;
using NServiceBus.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus
{
    public class AbpNServiceBusUnitOfWork : Behavior<IIncomingPhysicalMessageContext>
    {
        private ILog Logger = LogManager.GetLogger<AbpNServiceBusUnitOfWork>();

        public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            Logger.DebugFormat("Message: {0}", context.MessageId);

            // Get AbpSession
            IAbpSession session = IocManager.Instance.Resolve<IAbpSession>();
            Logger.DebugFormat("Message/AbpSession: {0}/{1}", context.MessageId, session.GetHashCode());

            AbpNServiceBusSession nsbSession = session as AbpNServiceBusSession;
            nsbSession.SetHeaders(context.MessageHeaders);

            // Get instance of UnitOfWorkManager
            IUnitOfWorkManager uowManager = IocManager.Instance.Resolve<IUnitOfWorkManager>();
            Logger.DebugFormat("Message/UowManager: {0}/{1}", context.MessageId, uowManager.GetHashCode());

            IUnitOfWorkCompleteHandle unitOfWork;
            try
            {
                // Start UnitOfWork
                unitOfWork = uowManager.Begin();
                Logger.DebugFormat("Message/UnitOfWork: {0}/{1}", context.MessageId, uowManager.Current.GetHashCode());

                // Call next step in pipeline
                await next();

                // Complete UnitOfWork if no exception is raised
                await unitOfWork.CompleteAsync();
            }
            catch (Exception)
            {
                // Does not Complete UnitOfWork
                throw;
            }
        }
    }
}
