using Abp.Dependency;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Transport.SQLServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus
{
    public static class AbpNServiceBusPersistenceExtensions
    {

        public static void ConfigureAbpNServiceBusDefaultPersistence(this EndpointConfiguration endpointConfiguration)
        {
            var config = IocManager.Instance.Resolve<AbpNServiceBusModuleConfig>();

            var nhConfiguration = new NHibernate.Cfg.Configuration
            {
                Properties =
               {
                    ["dialect"] = "NHibernate.Dialect.MsSql2012Dialect",
                    ["connection.provider"] = "NHibernate.Connection.DriverConnectionProvider",
                    ["connection.driver_class"] = "NHibernate.SqlAzure.SqlAzureClientDriver, NHibernate.SqlAzure",
                    ["default_schema"] = config.DatabaseSchemaName,
                    ["connection.connection_string"] = config.PersistenceConnectionString
               }
            };

            var persistence = endpointConfiguration.UsePersistence<NHibernatePersistence>();

            persistence.UseConfiguration(nhConfiguration);
        }

    }
}
