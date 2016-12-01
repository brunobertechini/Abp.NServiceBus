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

            // Default Peek Delay
            transport.WithPeekDelay(TimeSpan.FromSeconds(5));

            // Schemas
            transport.UseSchemaForQueue(config.AuditQueue, "dbo");
            transport.UseSchemaForQueue(config.ErrorQueue, "dbo");
            transport.UseSchemaForEndpoint(config.EndpointName, config.EndpointDatabaseSchema);
            
            if (!string.IsNullOrEmpty(config.TransportConnectionString))
                transport.ConnectionString(config.TransportConnectionString);

            //if (!string.IsNullOrEmpty(config.EndpointDatabaseSchema))
            //    transport.DefaultSchema(config.EndpointDatabaseSchema);
        }

    }
}
