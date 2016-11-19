using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus.WebJobs
{
    [DependsOn(typeof(AbpNServiceBusModule))]
    public class AbpNServiceBusWebJobsModule : AbpModule
    {
        public override void PreInitialize()
        {
            // Module Config
            IocManager.Register<AbpNServiceBusWebJobsModuleConfig>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        public override void PostInitialize()
        {
            // Start WebJob

        }
    }
}
