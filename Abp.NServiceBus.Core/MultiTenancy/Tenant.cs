using Abp.MultiTenancy;
using Abp.NServiceBus.Users;

namespace Abp.NServiceBus.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {
            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}