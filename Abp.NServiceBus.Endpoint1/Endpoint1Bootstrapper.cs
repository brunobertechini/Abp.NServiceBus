using Abp.Dependency;
using Abp.Modules;
using Abp.Runtime.Session;
using Castle.MicroKernel.Registration;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence;
using NServiceBus.Transport.SQLServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus
{
    [DependsOn(
        typeof(NServiceBusCoreModule),
        typeof(NServiceBusApplicationModule),
        typeof(NServiceBusDataModule),
        typeof(AbpNServiceBusModule)
    )]
    public class Endpoint1Bootstrapper : AbpModule
    {
        public const string EndpointName = "Abp.NServiceBus.Endpoint1";

        public override void PreInitialize()
        {
            var defaultConnString = ConfigurationManager.ConnectionStrings[NServiceBusConsts.ConnectionStringName].ConnectionString;

            Configuration.Modules.NServiceBus().Debug = true;
            Configuration.Modules.NServiceBus().EndpointName = EndpointName;
            Configuration.Modules.NServiceBus().LogDirectory = ConfigurationManager.AppSettings["LogDirPath"];
            Configuration.Modules.NServiceBus().TransportConnectionString = defaultConnString;
            Configuration.Modules.NServiceBus().PersistenceConnectionString = defaultConnString;
            Configuration.Modules.NServiceBus().LogLevel = LogLevel.Info;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
