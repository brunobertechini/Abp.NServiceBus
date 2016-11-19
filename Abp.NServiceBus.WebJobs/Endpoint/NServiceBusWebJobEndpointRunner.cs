using Abp;
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
    public class NServiceBusWebJobEndpointRunner<TStartupModule>
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
                host.Call<TStartupModule>();
                host.RunAndBlock();
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("WebJob TaskCancelled");
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
