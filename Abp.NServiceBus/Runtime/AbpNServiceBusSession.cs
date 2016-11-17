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
        private readonly AbpNServiceBusModuleConfig _config;

        private Dictionary<string, string> _headers = new Dictionary<string, string>();

        public AbpNServiceBusSession(IMultiTenancyConfig multiTenancy, AbpNServiceBusModuleConfig config)
        {
            _multiTenancy = multiTenancy;
            _config = config;

            if(_config.Debug)
                Logger.InfoFormat("Creating New Instance {0}", GetHashCode());
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
                    if (string.IsNullOrEmpty(value))
                        return null;

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
                    if (string.IsNullOrEmpty(value))
                        return null;

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
                    if (string.IsNullOrEmpty(value))
                        return null;

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
                    if (string.IsNullOrEmpty(value))
                        return null;

                    return Convert.ToInt64(value);
                }

                return null;
            }
        }

        public void SetHeaders(IReadOnlyDictionary<string, string> headers)
        {
            if(_config.Debug)
            {
                Logger.InfoFormat("Setting Headers");
                foreach (var header in headers)
                {
                    Logger.InfoFormat("{0}: {1}", header.Key, header.Value);
                }
            }
           
            _headers = headers.ToDictionary(x => x.Key, y => y.Value);
        }
    }

}
