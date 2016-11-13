using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Abp.NServiceBus.Authorization
{
    public class NServiceBusAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //Common permissions
            var pages = context.GetPermissionOrNull(PermissionNames.Pages);
            if (pages == null)
            {
                pages = context.CreatePermission(PermissionNames.Pages, L("Pages"));
            }

            var users = pages.CreateChildPermission(PermissionNames.Pages_Users, L("Users"));

            var blogs = pages.CreateChildPermission(PermissionNames.Pages_Blogs, L("Blogs"));

            //Host permissions
            var tenants = pages.CreateChildPermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, NServiceBusConsts.LocalizationSourceName);
        }
    }
}
