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
        public static TransportExtensions<SqlServerTransport> ConfigureAbpNServiceBusDefaultTransport(this EndpointConfiguration endpointConfiguration)
        {
            var config = IocManager.Instance.Resolve<AbpNServiceBusModuleConfig>();

            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();

            // Default Peek Delay
            transport.WithPeekDelay(config.TransportPeekDelay);

            // Schemas
            transport.UseSchemaForQueue(config.AuditQueue, "dbo");
            transport.UseSchemaForQueue(config.ErrorQueue, "dbo");
            transport.UseSchemaForEndpoint(config.EndpointName, config.EndpointDatabaseSchema);
            
            if (!string.IsNullOrEmpty(config.TransportConnectionString))
                transport.ConnectionString(config.TransportConnectionString);

            if(config.TransportTransactionMode.HasValue)
                transport.Transactions(config.TransportTransactionMode.Value);

            return transport;
        }
    }
}
