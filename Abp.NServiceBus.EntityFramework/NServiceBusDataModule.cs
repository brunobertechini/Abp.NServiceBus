using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using Abp.Zero.EntityFramework;
using Abp.NServiceBus.EntityFramework;

namespace Abp.NServiceBus
{
    [DependsOn(typeof(AbpZeroEntityFrameworkModule), typeof(NServiceBusCoreModule))]
    public class NServiceBusDataModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<NServiceBusDbContext>());

            Configuration.DefaultNameOrConnectionString = "Default";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
