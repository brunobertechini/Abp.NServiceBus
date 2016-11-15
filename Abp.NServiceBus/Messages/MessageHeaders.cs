using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus
{
    public static class MessageHeaders
    {
        private const string Separator = ".";

        /// <summary>
        /// Prefix for All Abp Headers
        /// </summary>
        public const string Prefix = "Abp";

        /// <summary>
        /// TenantId
        /// </summary>
        public const string TenantId = Prefix + Separator + "TenantId";

        /// <summary>
        /// Impersonated TenantId Impersonated
        /// </summary>
        public const string ImpersonatorTenantId = Prefix + Separator + "ImpersonatorTenantId";

        /// <summary>
        /// User Id
        /// </summary>
        public const string UserId = Prefix + Separator + "UserId";

        /// <summary>
        /// Impersonated User Id 
        /// </summary>
        public const string ImpersonatorUserId = Prefix + Separator + "ImpersonatorUserId";

    }
}
