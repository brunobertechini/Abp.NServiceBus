using Abp;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Castle.Core.Logging;
using Castle.Facilities.Logging;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abp.NServiceBus.WebJobs
{
    public class NServiceBusWebJobEndpoint
    {
        [NoAutomaticTrigger]
        public void Start(TextWriter log, CancellationToken cancellationToken, Type startupModule)
        {
            // Initialize Abp infrastructure
            var bootstrapper = AbpBootstrapper.Create(startupModule);
            bootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));
            bootstrapper.Initialize();

            // Lock until cancelled
            Console.WriteLine("Lock Thread using WebJobUtils...");
            IocManager.Instance.Resolve<WebJobUtils>().RunAndWait();

            // Dispose Abp
            Console.WriteLine("Thread released, disposing Abp...");
            bootstrapper.Dispose();

            Console.WriteLine("WebJobEndpoint: Abp Disposed Successfully.");
        }
    }
}
