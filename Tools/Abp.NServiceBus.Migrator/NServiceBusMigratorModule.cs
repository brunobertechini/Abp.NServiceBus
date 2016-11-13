using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.NServiceBus.EntityFramework;

namespace Abp.NServiceBus.Migrator
{
    [DependsOn(typeof(NServiceBusDataModule))]
    public class NServiceBusMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<NServiceBusDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}