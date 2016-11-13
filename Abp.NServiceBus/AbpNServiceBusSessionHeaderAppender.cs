using Abp.Runtime.Session;
using NServiceBus.MessageMutator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus
{
    public class AbpNServiceBusSessionHeaderAppender : IMutateOutgoingTransportMessages
    {
        private readonly IAbpSession _session;

        public AbpNServiceBusSessionHeaderAppender(IAbpSession session)
        {
            _session = session;
        }

        public Task MutateOutgoing(MutateOutgoingTransportMessageContext context)
        {
            var outgoingHeaders = context.OutgoingHeaders;

            var tenantId = (_session.TenantId != null) ? _session.TenantId.Value.ToString() : "0";
            var impersonatorTenantId = (_session.ImpersonatorTenantId != null) ? _session.ImpersonatorTenantId.Value.ToString() : string.Empty;
            var userId = (_session.UserId != null) ? _session.UserId.Value.ToString() : "0";
            var impersonatorUserId = (_session.ImpersonatorUserId != null) ? _session.ImpersonatorUserId.Value.ToString() : string.Empty;

            outgoingHeaders.Add(MessageHeaders.TenantId, tenantId);
            outgoingHeaders.Add(MessageHeaders.UserId, userId);

            outgoingHeaders.Add(MessageHeaders.ImpersonatorTenantId, impersonatorTenantId);
            outgoingHeaders.Add(MessageHeaders.ImpersonatorUserId, impersonatorUserId);

            return Task.FromResult(0);
        }
    }
}
