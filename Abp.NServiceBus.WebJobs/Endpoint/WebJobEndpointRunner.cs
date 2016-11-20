using Abp;
using Abp.Dependency;
using Abp.Modules;
using Castle.Facilities.Logging;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Abp.NServiceBus.WebJobs
{
    public class WebJobEndpointRunner<TStartupModule>
        where TStartupModule : AbpModule
    {
        public void Run()
        {
            // TODO Validate if TStartupModule depends on NServiceBus/WebJobs Module

            // WebJob Host
            JobHost host;

            // To run webjobs locally, can't use storage emulator 
            if (Debugger.IsAttached)
            {
                var configuration = new JobHostConfiguration
                {
                    DashboardConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsDashboard"].ConnectionString,
                    StorageConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString
                };
                host = new JobHost(configuration);
            }
            // for production, use DashboardConnectionString and StorageConnectionString defined at Azure website
            else
            {
                host = new JobHost();
            }

            try
            {
                var bootstrapper = AbpBootstrapper.Create<TStartupModule>();
                bootstrapper.IocManager.IocContainer.AddFacility<LoggingFacility>(f => f.UseLog4Net().WithConfig("log4net.config"));

                host.Call(typeof(WebJobEndpoint).GetMethod("Start"), new { bootstrapper = bootstrapper });

                host.RunAndBlock();

                Console.WriteLine("Disposing Abp/NServiceBus...");
                bootstrapper.Dispose();
                Console.WriteLine("Abp disposed successfully.");

                if (Debugger.IsAttached)
                    Console.Read();
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine("TaskCancelled Exception");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal Exception");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw ex;
            }
        }

    }
}
