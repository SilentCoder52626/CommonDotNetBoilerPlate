using DomainModule.Dto.User;
using DomainModule.Entity;
using DomainModule.Exceptions;
using DomainModule.RepositoryInterface;
using DomainModule.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using NToastNotify.Helpers;
using WebApp.ViewModel;
using WebApp.Extensions;
using WebApp.Helper;
using DomainModule.ServiceInterface.Email;
using DomainModule.Dto.Email;
using System.Security.Cryptography;

namespace WebApp.Areas.Account.Controllers
{
    [Area("Account")]
    public class UserController : Controller
    {
        private readonly UserRepositoryInterface _userRepo;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IToastNotification _notify;
        private readonly UserServiceInterface _userService;
        private readonly IEmailSenderService _emailService;
        public UserController(UserRepositoryInterface userRepository,
            IToastNotification notify,
            UserServiceInterface userService,
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager
,
            IEmailSenderService emailService)
        {
            _userRepo = userRepository;
            _notify = notify;
            _userService = userService;
            _roleManager = roleManager;
            _userManager = userManager;
            _emailService = emailService;
        }
        [Authorize(Policy = "User-View")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepo.GetQueryable().Where(a => a.Type != DomainModule.Entity.User.TypeSuperAdmin).ToListAsync().ConfigureAwait(true);
            var userIndexViewModels = new List<UserIndexViewModel>();
            var i = 1;
            foreach (var user in users)
            {
                userIndexViewModels.Add(new UserIndexViewModel
                {
                    SN = i,
                    Id = user.Id,
                    Name = user.Name,
                    EmailAddress = user.Email,
                    MobileNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Status = user.Status,
                    Type = user.Type
                });
                i++;
            }
            return View(userIndexViewModels);
        }
        [Authorize(Policy = "User-Create")]
        public async Task<IActionResult> Create()
        {
            var roles = await _roleManager.Roles.Where(a => a.Name != "SuperAdmin").ToListAsync();
            ViewBag.RoleList = new SelectList(roles, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            try
            {
                var createDto = new UserDto()
                {
                    Name = model.Name,
                    EmailAddress = model.EmailAddress,
                    MobileNumber = model.MobileNumber,
                    Password = model.Password,
                    UserName = model.UserName,
                    Type = DomainModule.Entity.User.TypeGeneral,
                    CurrentSiteDomain = $"{Request.Scheme}://{Request.Host}",
                    Roles = model.Roles ?? new List<string>(),
                };
                var userReponse = await _userService.Create(createDto);

                _notify.AddSuccessToastMessage("User created succesfully. Please check user email and confirm user account");

                if (userReponse.EmailAddress != null)
                {
                    var htmlString = $"<p>Dear {userReponse.Name}({userReponse.UserName}),<br/>&emsp;&emsp; Please follow <a href='{userReponse.EmailConfirmationLink}' target='_blank'> this Link</a> to acitvate your account in Common World.<br/><br/>Regards,<br/>CommonWorld</p>";
                    var message = new MessageDto(new string[] { userReponse.EmailAddress }, "Please confirm you account.", htmlString, null);
                    await _emailService.SendEmailAsync(message).ConfigureAwait(true);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);

            }
            var roles = await _roleManager.Roles.Where(a => a.Name != "SuperAdmin").ToListAsync();
            ViewBag.RoleList = new SelectList(roles, "Id", "Name");
            return View(model);
        }
        [Authorize(Policy = "User-Update")]
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                var roles = await _roleManager.Roles.Where(a => a.Name != "SuperAdmin").ToListAsync();
                ViewBag.RoleList = new SelectList(roles, "Id", "Name");
                var user = await _userManager.FindByIdAsync(id).ConfigureAwait(true) ?? throw new UserNotFoundException();
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = await _roleManager.Roles.Where(a => userRoles.Contains(a.Name)).ToListAsync().ConfigureAwait(true);

                var userUpdateModel = new UserEditViewModel()
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.UserName,
                    EmailAddress = user.Email,
                    MobileNumber = user.PhoneNumber,
                    Roles = allRoles.Select(a => a.Id).ToList(),

                };
                return View(userUpdateModel);
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            try
            {
                var editDto = new UserEditDto()
                {
                    Id = model.Id,
                    Name = model.Name,
                    EmailAddress = model.EmailAddress,
                    MobileNumber = model.MobileNumber,
                    UserName = model.UserName,
                    Roles = model.Roles ?? new List<string>()
                };
                await _userService.Edit(editDto);

                _notify.AddSuccessToastMessage("Updated Successfully.");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);

            }
            var roles = await _roleManager.Roles.Where(a => a.Name != "SuperAdmin").ToListAsync();
            ViewBag.RoleList = new SelectList(roles, "Id", "Name");
            return View(model);
        }
        public async Task<IActionResult> UserProfile()
        {
            var userId = this.GetCurrentUserId();
            return RedirectToAction(nameof(Edit), new { id = userId });
        }

        public async Task<IActionResult> Activate(string id)
        {
            try
            {
                await _userService.Activate(id).ConfigureAwait(true);

                _notify.AddSuccessToastMessage("Activated Successfully");
                var user = await _userRepo.GetByIdStringAsync(id);
                if (user.Email != null)
                {
                    var message = new MessageDto(new string[] { user.Email }, "Your account have been activated", "Your account have been activated.", null);
                    await _emailService.SendEmailAsync(message).ConfigureAwait(true);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
                return RedirectToAction(nameof(Index));
            }

        }
        [Authorize(Policy = "User-Lock")]
        public async Task<IActionResult> Deactivate(string id)
        {
            try
            {
                await _userService.Deactivate(id).ConfigureAwait(true);
                _notify.AddSuccessToastMessage("Deactivated Successfully");
                var user = await _userRepo.GetByIdStringAsync(id);
                if (user.Email != null)
                {
                    var message = new MessageDto(new string[] { user.Email }, "Your account have been deactivated", "Your account have been deactivated.", null);
                    await _emailService.SendEmailAsync(message).ConfigureAwait(true);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _notify.AddErrorToastMessage(ex.Message);
                return RedirectToAction(nameof(Index));
            }

        }
        [Authorize("User-ResetPassword")]
        public async Task<IActionResult> ResetPassword(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id) ?? throw new UserNotFoundException();

                if (user.Email != null)
                {
                    string new_password = RandomAlphaNumericHelper.Random(8);

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, new_password).ConfigureAwait(true);
                    if (result.Succeeded)
                    {
                        var htmlString = $"<p>Dear {user.Name}({user.UserName}),<br/><br/>&emsp;&emsp; Please use <b>{new_password}</b> as new password to login to your account.  <br/><br/>Regards,<br/>CommonWorld</p>";
                        var message = new MessageDto(new string[] { user.Email }, "User Password Reset.", htmlString, null);
                        await _emailService.SendEmailAsync(message).ConfigureAwait(true);
                    }
                }
                else
                {
                    throw new Exception("Email address of the user is missing.");
                }
                _notify.AddSuccessToastMessage("Password Reset Email Send Succussfully.");

            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex.Message, ex);
                _notify.AddErrorToastMessage(ex.Message);
            }
            return RedirectToAction(nameof(Index));

        }


        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            try
            {
                var userId = this.GetCurrentUserId();
                var user = await _userManager.FindByIdAsync(userId) ?? throw new UserNotFoundException();
                if (ModelState.IsValid)
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.OldPasword, model.NewPassword).ConfigureAwait(true);
                    if (result.Succeeded)
                    {
                        _notify.AddSuccessToastMessage("Password Changed Successfully");
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                    else
                    {
                        _notify.AddInfoToastMessage(string.Join("</br>", result.Errors.Select(a => a.Description).ToList()));
                    }

                }
            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex.Message, ex);
                _notify.AddErrorToastMessage(ex.Message);
            }
            return View(model);
        }

    }
}
