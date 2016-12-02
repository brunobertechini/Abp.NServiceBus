using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus
{
    // TODO Implementar DataAnnotation validation
    public class AbpNServiceBusModuleConfig
    {
        public AbpNServiceBusModuleConfig()
        {
            Debug = false;
            EndpointName = null;
            EndpointConfiguration = null;
            LogDirectory = null;
            EndpointDatabaseSchema = "dbo";
            ImmediateRetries = 0;
            DelayedRetries = 0;
            DelayedRetriesTimeIncreaseInSeconds = 30;
            LogLevel = null;
            AuditQueue = "Endpoint.Audit";
            ErrorQueue = "Endpoint.Error";
            UseOutbox = true;
            MaximumConcurrencyLevel = null;
            UseEntityFrameworkUnitOfWork = true;
        }

        /// <summary>
        /// Enable Debug
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Defines the Endpoint Name. Required.
        /// </summary>
        public string EndpointName { get; set; }

        /// <summary>
        /// Endpoint Configuration with Default Values
        /// </summary>
        public EndpointConfiguration EndpointConfiguration { get; set; }

        /// <summary>
        /// Directory where the NServiceBus Logs will be stored. Default: null (use root directory)
        /// </summary>
        public string LogDirectory { get; set; }

        /// <summary>
        /// Log Level
        /// </summary>
        public LogLevel? LogLevel { get; set; }

        /// <summary>
        /// Custom Database Schema Name. Default: "dbo".
        /// </summary>
        public string EndpointDatabaseSchema { get; set; }

        /// <summary>
        /// Number of ImmediateRetries to perform. Default: 0.
        /// </summary>
        public int ImmediateRetries { get; set; }

        /// <summary>
        /// Number of DelayedRetries to perform. Default: 0.
        /// </summary>
        public int DelayedRetries { get; set; }

        /// <summary>
        /// TimeIncrease in seconds between each DelayedRetries to perform. Default: 30.
        /// </summary>
        public int DelayedRetriesTimeIncreaseInSeconds { get; set; }

        /// <summary>
        /// Connection string for SqlServer Transport: Required.
        /// </summary>
        public string TransportConnectionString { get; set; }

        /// <summary>
        /// Connection string for NHibernate Persistence: Required.
        /// </summary>
        public string PersistenceConnectionString { get; set; }

        /// <summary>
        /// Audit Queue Name. Default: 'Endpoint.Audit'.
        /// </summary>
        public string AuditQueue { get; set; }

        /// <summary>
        /// Error Queue Name. Default: 'Endpoint.Error'.
        /// </summary>
        public string ErrorQueue { get; set; }

        /// <summary>
        /// Enable NServiceBus Outbox Feature. Default: true.
        /// </summary>
        public bool UseOutbox { get; set; }

        /// <summary>
        /// Maximum Concurrency Level. Default: null (NServiceBus Transport will choose best value)
        /// @see https://docs.particular.net/nservicebus/operations/tuning
        /// </summary>
        public int? MaximumConcurrencyLevel { get; set; }

        /// <summary>
        /// Use a EF UoW from Abp (Default: true)
        /// Must set UnitOfWork.IsTransactionalFalse before
        /// </summary>
        public bool UseEntityFrameworkUnitOfWork  { get; set; }

        public bool DoNotUseDefaultTransport { get; set; }

        public bool DoNotUseDefaultPersistence { get; set; }

    }

}
