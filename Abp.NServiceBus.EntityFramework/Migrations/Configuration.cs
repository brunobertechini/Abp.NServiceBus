using System.Data.Entity.Migrations;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using Abp.NServiceBus.Migrations.SeedData;
using EntityFramework.DynamicFilters;

namespace Abp.NServiceBus.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<NServiceBus.EntityFramework.NServiceBusDbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "NServiceBus";
        }

        protected override void Seed(NServiceBus.EntityFramework.NServiceBusDbContext context)
        {
            context.DisableAllFilters();

            if (Tenant == null)
            {
                //Host seed
                new InitialHostDbBuilder(context).Create();

                //Default tenant seed (in host database).
                new DefaultTenantCreator(context).Create();
                new TenantRoleAndUserBuilder(context, 1).Create();
            }
            else
            {
                //You can add seed for tenant databases and use Tenant property...
            }

            context.SaveChanges();
        }
    }
}
