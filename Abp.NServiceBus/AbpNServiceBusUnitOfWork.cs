using Abp.Domain.Uow;
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
        //private AbpNServiceBusSession _session;
        //private IUnitOfWorkManager _uowManager;

        public override Task Invoke(IIncomingPhysicalMessageContext context, Func<Task> next)
        {
            //Console.WriteLine($"AbpUnitOfWorkBehavior: Processing Message {context.Message.MessageId} ");

            //_session = IocManager.Instance.Resolve<IAbpSession>() as AbpNServiceBusSession;
            //_uowManager = IocManager.Instance.Resolve<IUnitOfWorkManager>();

            //try
            //{
            //    _session.SetHeaders(context.MessageHeaders.ToDictionary(x => x.Key, y => y.Value));

            //    var handle = _uowManager.Begin();

            //    handle.Complete();

            //    await next();

            //    await _uowManager.Current.SaveChangesAsync();

            //    Console.Out.WriteLine($"AbpUnitOfWorkBehavior: Message {context.MessageId}: Session {_session.GetHashCode()} UOW {_uowManager.GetHashCode()} was committed");
            //}
            //catch (Exception)
            //{
            //    Console.Out.WriteLine($"AbpUnitOfWorkBehavior: Message {context.MessageId}: Session {_session.GetHashCode()} UOW {_uowManager.GetHashCode()} was rolled back");
            //    throw;
            //}

            return next();
        }
    }
}
