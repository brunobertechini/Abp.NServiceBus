using Abp.Configuration.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus.WebJobs
{
    public static class AbpNServiceBusWebJobsModuleExtensions
    {
        public static AbpNServiceBusWebJobsModuleConfig NServiceBusWebJob(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<AbpNServiceBusWebJobsModuleConfig>();
        }
    }
}
