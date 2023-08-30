using DomainModule.Dto;
using DomainModule.Entity;
using DomainModule.Exceptions;
using DomainModule.RepositoryInterface;
using DomainModule.ServiceInterface;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace DomainModule.Service
{
    public class RoleService : RoleServiceInterface
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public RoleService(RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager,
            IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task AssignAllPermissionOfModule(string roleId, string module)
        {

            using (var tx = await _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false) ?? throw new RoleNotFoundException();
                    var allPermissionsOfModule = PermissionHelper.GetPermission().PermissionDictionary.Where(a => a.Key == module).SelectMany(p => p.Value.Select(i => $"{p.Key}-{i}"));
                    var rolePermissions = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);

                    foreach (var permission in allPermissionsOfModule)
                    {
                        if (!rolePermissions.Any(x => x.Type == Permission.PermissionClaimType && x.Value == permission))
                        {
                            var roleClaim = await _roleManager.AddClaimAsync(role, new Claim(Permission.PermissionClaimType, permission)).ConfigureAwait(false);
                            if (!roleClaim.Succeeded) throw new CustomException("Error to Assign Permission");
                        }
                    }
                    await _unitOfWork.CompleteAsync().ConfigureAwait(false);
                    await tx.CommitAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync().ConfigureAwait(false);
                    throw;

                }
            }

        }

        public async Task AssignPermission(string roleId, string permission)
        {

            using (var tx = await _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false) ?? throw new RoleNotFoundException();
                    var rolePermissions = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
                    if (!rolePermissions.Any(x =>
                    x.Type == Permission.PermissionClaimType &&
                             x.Value == permission))
                    {
                        var roleClaim = await _roleManager.AddClaimAsync(role, new Claim(Permission.PermissionClaimType, permission)).ConfigureAwait(false);
                        if (!roleClaim.Succeeded) throw new CustomException("Error to Assign Permission");
                    }
                    await _unitOfWork.CompleteAsync().ConfigureAwait(false);
                    await tx.CommitAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync().ConfigureAwait(false);
                    throw;
                }
            }

        }


        public async Task Create(RoleDto dto)
        {
            using (var tx = await _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    var identityRole = new IdentityRole()
                    {
                        Name = dto.RoleName
                    };
                    var response = await _roleManager.CreateAsync(identityRole).ConfigureAwait(false);
                    if (!response.Succeeded)
                    {
                        throw new CustomException(string.Join("</br>", response.Errors.Select(a => a.Description).ToList()));
                    }
                    await _unitOfWork.CompleteAsync().ConfigureAwait(false);
                    await tx.CommitAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync().ConfigureAwait(false);
                    throw;
                }
            }
        }

        public async Task Edit(RoleEditDto dto)
        {
            using (var tx = await _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(dto.Id).ConfigureAwait(false) ?? throw new RoleNotFoundException();
                    role.Name = dto.Name;
                    var response = await _roleManager.UpdateAsync(role).ConfigureAwait(false);
                    if (!response.Succeeded) throw new CustomException(string.Join("</br>", response.Errors.SelectMany(a => a.Description).ToList()));
                    await _unitOfWork.CompleteAsync().ConfigureAwait(false);
                    await tx.CommitAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync().ConfigureAwait(false);
                    throw;
                }
            }
        }

        public async Task<PermissionDto> GetALLPermissions(string roleId)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(roleId) ?? throw new RoleNotFoundException();
                var rolesPermission = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
                var permissionDto = new PermissionDto() { RoleId = roleId };
                var AllPermissions = PermissionHelper.GetPermission().PermissionDictionary.OrderBy(a => a.Key);

                foreach (var permission in AllPermissions)
                {
                    var moduleWisePermission = new ModuleWisePermissionDto { Module = permission.Key };
                    foreach (var data in permission.Value.OrderBy(a => a))
                    {

                        moduleWisePermission.PermissionData.Add(new PermissionValues()
                        {
                            IsAssigned = rolesPermission.Any(x =>
                           x.Type == Permission.PermissionClaimType &&
                             x.Value == permission.Key + "-" + data),
                            Value = data
                        });
                    }
                    permissionDto.Permissions.Add(moduleWisePermission);
                }

                return permissionDto;

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task UnAssignPermission(string roleId, string permission)
        {
            using (var tx = await _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {

                    var role = await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false) ?? throw new RoleNotFoundException();
                    var rolePermissions = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);
                    if (rolePermissions.Any(x =>
                    x.Type == Permission.PermissionClaimType &&
                             x.Value == permission))
                    {
                        var claim = rolePermissions.Where(x =>
                    x.Type == Permission.PermissionClaimType &&
                             x.Value == permission).First();

                        var response = await _roleManager.RemoveClaimAsync(role, claim);
                        if (!response.Succeeded) throw new CustomException("Error to UnAssign Permission");
                    }
                    await _unitOfWork.CompleteAsync().ConfigureAwait(false);
                    await tx.CommitAsync();

                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync().ConfigureAwait(false);
                    throw;
                }
            }
        }

        public async Task UnAssignPermissionOfModule(string roleId, string module)
        {

            using (var tx = await _unitOfWork.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false) ?? throw new RoleNotFoundException();
                    var allPermissionsOfModule = PermissionHelper.GetPermission().PermissionDictionary.Where(a => a.Key == module).SelectMany(p => p.Value.Select(i => $"{p.Key}-{i}"));
                    var rolePermissions = await _roleManager.GetClaimsAsync(role).ConfigureAwait(false);

                    foreach (var permission in allPermissionsOfModule)
                    {
                        if (rolePermissions.Any(x => x.Type == Permission.PermissionClaimType && x.Value == permission))
                        {
                            var claim = rolePermissions.Where(x =>
                    x.Type == Permission.PermissionClaimType &&
                             x.Value == permission).First();

                            var response = await _roleManager.RemoveClaimAsync(role, claim);

                            if (!response.Succeeded) throw new CustomException("Error to Assign Permission");
                        }
                    }
                    await _unitOfWork.CompleteAsync().ConfigureAwait(false);
                    await tx.CommitAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    await tx.RollbackAsync().ConfigureAwait(false);

                    throw;
                }
            }
        }
    }
}
