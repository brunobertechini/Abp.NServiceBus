using Abp.Configuration.Startup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus
{
    public static class AbpNServiceBusModuleExtensions
    {
        public static AbpNServiceBusModuleConfig ErpCommerceNServiceBusModule(this IModuleConfigurations moduleConfigurations)
        {
            return moduleConfigurations.AbpConfiguration.Get<AbpNServiceBusModuleConfig>();
        }
    }
}
