using Abp.Modules;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus.WebJobs
{
    public static class NServiceBusWebJobEndpointExtensions
    {
        /// <summary>
        /// Call WebJobEndpoint using the specified parameter as Abp StartupModule
        /// </summary>
        /// <typeparam name="TStartupModule">Abp Startup Module</typeparam>
        public static void Call<TStartupModule>(this JobHost host) 
            where TStartupModule : AbpModule
        {
            host.Call(typeof(NServiceBusWebJobEndpoint).GetMethod("Start"), new { startupModule = typeof(TStartupModule) });
        }
    }
}
