using Abp.Dependency;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.MultiTenancy;
using NServiceBus;
using Abp.Configuration.Startup;
using NServiceBus.Logging;

namespace Abp.NServiceBus
{
    /// <summary>
    /// Custom Session for NServiceBus Endpoints
    /// </summary>
    public class AbpNServiceBusSession : IAbpSession
    {
        private readonly ILog Logger = LogManager.GetLogger<AbpNServiceBusSession>();

        private readonly IMultiTenancyConfig _multiTenancy;
        private Dictionary<string, string> _headers = new Dictionary<string, string>();

        public AbpNServiceBusSession(IMultiTenancyConfig multiTenancy)
        {
            _multiTenancy = multiTenancy;
        }

        /// <summary>
        /// NServiceBus Message Id
        /// </summary>
        public string MessageId { get; set; }

        public MultiTenancySides MultiTenancySide
        {
            get
            {
                return _multiTenancy.IsEnabled && !TenantId.HasValue
                    ? MultiTenancySides.Host
                    : MultiTenancySides.Tenant;
            }
        }

        public int? TenantId
        {
            get
            {
                string value;
                if (_headers.TryGetValue(MessageHeaders.TenantId, out value))
                {
                    return Convert.ToInt32(value);
                }

                return null;
            }
        }

        public int? ImpersonatorTenantId
        {

            get
            {
                string value;
                if (_headers.TryGetValue(MessageHeaders.ImpersonatorTenantId, out value))
                {
                    return Convert.ToInt32(value);
                }

                return null;
            }
        }

        public long? UserId
        {
            get
            {
                string value;
                if (_headers.TryGetValue(MessageHeaders.UserId, out value))
                {
                    return Convert.ToInt64(value);
                }

                return null;
            }
        }

        public long? ImpersonatorUserId
        {
            get
            {
                string value;
                if (_headers.TryGetValue(MessageHeaders.ImpersonatorUserId, out value))
                {
                    return Convert.ToInt64(value);
                }

                return null;
            }
        }

        public void SetHeaders(Dictionary<string, string> headers)
        {
            _headers = headers;
        }
    }

}
