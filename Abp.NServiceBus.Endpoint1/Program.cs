using Abp.Castle.Logging.Log4Net;
using Abp.Dependency;
using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using NServiceBus;
using NServiceBus.Persistence;
using NServiceBus.Transport.SQLServer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus
{
    class Program
    {
        static void Main()
        {
            Console.Title = Endpoint1Bootstrapper.EndpointName;

            // Initialize Abp infrastructure
            var bootstrapper = AbpBootstrapper.Create<Endpoint1Bootstrapper>();
            bootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(
                f => f.UseAbpLog4Net().WithConfig("log4net.config")
            );
            bootstrapper.Initialize();

            // Run and Block
            Console.ReadKey();

            // Dispose Abp
            Console.WriteLine("Disposing Abp...");
            bootstrapper.Dispose();

            Console.WriteLine("Abp Disposed Successfully.");
        }
    }
}
