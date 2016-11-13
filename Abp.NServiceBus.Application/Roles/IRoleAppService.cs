using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.NServiceBus.Roles.Dto;

namespace Abp.NServiceBus.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
    }
}
