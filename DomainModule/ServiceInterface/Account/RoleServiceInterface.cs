using DomainModule.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.ServiceInterface
{
    public interface RoleServiceInterface
    {
        Task Create(RoleDto dto);
        Task Edit(RoleEditDto dto);
        Task<PermissionDto> GetALLPermissions(string roleId);
        Task AssignPermission(string roleId, string permission);
        Task UnAssignPermission(string roleId, string permission);
        Task AssignAllPermissionOfModule(string roleId, string module);
        Task UnAssignPermissionOfModule(string roleId, string module);
    }
}
