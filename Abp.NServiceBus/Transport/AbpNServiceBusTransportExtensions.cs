using Abp.Dependency;
using NServiceBus;
using NServiceBus.Transport.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus
{
    public static class AbpNServiceBusTransportExtensions
    {

        public static void ConfigureAbpNServiceBusDefaultTransport(this EndpointConfiguration endpointConfiguration)
        {
            var config = IocManager.Instance.Resolve<AbpNServiceBusModuleConfig>();

            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();

            if (!string.IsNullOrEmpty(config.TransportConnectionString))
                transport.ConnectionString(config.TransportConnectionString);

            if (!string.IsNullOrEmpty(config.DatabaseSchemaName))
                transport.DefaultSchema(config.DatabaseSchemaName);
        }

    }
}
