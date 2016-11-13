using System.Linq;
using Abp.NServiceBus.EntityFramework;
using Abp.NServiceBus.MultiTenancy;

namespace Abp.NServiceBus.Migrations.SeedData
{
    public class DefaultTenantCreator
    {
        private readonly NServiceBusDbContext _context;

        public DefaultTenantCreator(NServiceBusDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateUserAndRoles();
        }

        private void CreateUserAndRoles()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == Tenant.DefaultTenantName);
            if (defaultTenant == null)
            {
                _context.Tenants.Add(new Tenant {TenancyName = Tenant.DefaultTenantName, Name = Tenant.DefaultTenantName});
                _context.SaveChanges();
            }
        }
    }
}
