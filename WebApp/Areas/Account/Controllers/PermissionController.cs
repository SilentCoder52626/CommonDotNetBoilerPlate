using DomainModule.ServiceInterface;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.ViewModel;
using System.Net;

namespace WebApp.Areas.Account.Controllers
{
    [Area("Account")]
    public class PermissionController : Controller
    {
        private readonly RoleServiceInterface _roleService;

        public PermissionController(RoleServiceInterface roleService)
        {
            _roleService = roleService;
        }
        public async Task<IActionResult> Index(string RoleId)
        {
            try
            {
                var allPermissions = await _roleService.GetALLPermissions(RoleId).ConfigureAwait(true);
                var permissionViewModel = new PermissionViewModel { RoleId = RoleId };
                foreach(var permission in allPermissions.Permissions)
                {
                    var moduleWisePermission = new ModuleWisePermissionViewModel { Module = permission.Module };
                    foreach(var data in permission.PermissionData)
                    {
                        moduleWisePermission.PermissionData.Add(new PermissionValuesViewModel
                        {
                            IsAssigned = data.IsAssigned,
                            Value = data.Value
                        });
                    }
                    permissionViewModel.Permissions.Add(moduleWisePermission);
                }
                return View(permissionViewModel);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        public async Task<IActionResult> LoadPermissionView(string RoleId)
        {
            try
            {
                var allPermissions = await _roleService.GetALLPermissions(RoleId).ConfigureAwait(true);
                var permissionViewModel = new PermissionViewModel { RoleId = RoleId };
                foreach (var permission in allPermissions.Permissions)
                {
                    var moduleWisePermission = new ModuleWisePermissionViewModel { Module = permission.Module };
                    foreach (var data in permission.PermissionData)
                    {
                        moduleWisePermission.PermissionData.Add(new PermissionValuesViewModel
                        {
                            IsAssigned = data.IsAssigned,
                            Value = data.Value
                        });
                    }
                    permissionViewModel.Permissions.Add(moduleWisePermission);
                }
                return PartialView("~/Areas/Account/Views/Permission/_AssignPermissionView.cshtml", permissionViewModel);
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel { Status = StatusType.error.ToString(), IsSuccess = true, Message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AssignPermission(string roleId,string permission)
        {
            try
            {
                  await _roleService.AssignPermission(roleId, permission).ConfigureAwait(true);
                return Json(new ResponseModel { Status = StatusType.success.ToString(), IsSuccess = true, Message = $"Permission {permission} assinged successfully" });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel { Status = StatusType.error.ToString(), IsSuccess = true, Message = ex.Message });
            }
        }
        public async Task<IActionResult> UnAssignPermission(string roleId, string permission)
        {
            try
            {
                await _roleService.UnAssignPermission(roleId, permission).ConfigureAwait(true);
                return Json(new ResponseModel { Status = StatusType.success.ToString(), IsSuccess = true, Message = $"Permission {permission} unassinged successfully" });
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel { Status = StatusType.error.ToString(), IsSuccess = true, Message = ex.Message });
            }
        }

        public async Task<IActionResult> AssignAllPermissionOfModule(string roleId, string module)
        {
            try
            {
                await _roleService.AssignAllPermissionOfModule(roleId, module).ConfigureAwait(true);
                return Json(new ResponseModel { Status = StatusType.success.ToString(), IsSuccess = true, Message = $"Permission {module} assinged successfully"});
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel { Status = StatusType.error.ToString(), IsSuccess = true, Message = ex.Message });
            }
        }
        public async Task<IActionResult> UnAssignAllPermissionOfModule(string roleId, string module)
        {
            try
            {
                await _roleService.UnAssignPermissionOfModule(roleId, module).ConfigureAwait(true);
                return Json(new ResponseModel { Status = StatusType.success.ToString(), IsSuccess = true, Message = $"Module {module} unassinged successfully"});
            }
            catch (Exception ex)
            {
                return Json(new ResponseModel { Status = StatusType.error.ToString(), IsSuccess = true, Message = ex.Message });
            }
        }

    }
}
