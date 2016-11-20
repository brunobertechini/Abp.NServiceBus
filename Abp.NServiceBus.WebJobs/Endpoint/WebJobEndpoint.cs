using Abp;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Castle.Core.Logging;
using Castle.Facilities.Logging;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abp.NServiceBus.WebJobs
{
    public class WebJobEndpoint
    {
        private bool _isRunning = false;

        [NoAutomaticTrigger]
        public void Start(TextWriter log, CancellationToken cancellationToken, AbpBootstrapper bootstrapper)
        {
            if (!Debugger.IsAttached)
                Console.WriteLine("Running in Azure Environment");

            Console.WriteLine("Monitoring Shutdown file: " + Environment.GetEnvironmentVariable("WEBJOBS_SHUTDOWN_FILE"));

            Console.WriteLine("Initializing Abp");
            bootstrapper.Initialize();

            _isRunning = true;
            new WebJobsShutdownWatcher().Token.Register(() =>
            {
                _isRunning = false;
            });

            while(_isRunning)
            {
                Thread.Sleep(3000);
            }
        }
    }
}
