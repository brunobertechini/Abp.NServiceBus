using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
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
        public override async Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            // Get instance of AbpNServiceBusSession
            IAbpSession session = IocManager.Instance.Resolve<IAbpSession>();
            AbpNServiceBusSession nsbSession = session as AbpNServiceBusSession;

            // Set AbpSession properties from Headers
            nsbSession.SetHeaders(context.MessageHeaders.ToDictionary(x => x.Key, y => y.Value));

            // Get instance of UnitOfWorkManager
            IUnitOfWorkManager uowManager = IocManager.Instance.Resolve<IUnitOfWorkManager>();
            IUnitOfWorkCompleteHandle unitOfWork;

            try
            {
                // Start UnitOfWork if
                unitOfWork = uowManager.Begin();

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
