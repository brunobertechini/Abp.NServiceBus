using Abp.Authorization;
using Abp.NServiceBus.Authorization.Roles;
using Abp.NServiceBus.MultiTenancy;
using Abp.NServiceBus.Users;

namespace Abp.NServiceBus.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
