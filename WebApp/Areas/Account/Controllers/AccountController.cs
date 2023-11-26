using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DomainModule.Dto.User;
using DomainModule.Entity;
using DomainModule.Exceptions;
using DomainModule.ServiceInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using WebApp.ViewModel;
using ServiceModule.Service;
using WebApp.Helper;
using DomainModule.Dto.Email;
using NETCore.MailKit.Core;
using DomainModule.ServiceInterface.Email;

namespace WebApp.Areas.Account.Controllers
{
	[Area("Account")]
	[AllowAnonymous]
	public class AccountController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly UserServiceInterface _userService;
		private readonly SignInManager<User> _signInManager;
		private readonly IToastNotification _notify;
		private readonly IEmailSenderService _emailService;
        public AccountController(
            IToastNotification notify,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
           UserServiceInterface userService,
           IEmailSenderService emailService)
        {
            _notify = notify;
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _emailService = emailService;
        }

        public async Task<IActionResult> Login(string ReturnUrl = "/Home/Index")
		{
			var loginModel = new LoginViewModel()
			{
				ReturnUrl = ReturnUrl,
				ExternalProviders = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
			};

			return View(loginModel);
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			try
			{
				if (ModelState.IsValid)
				{

					var user = await _userManager.FindByEmailAsync(model.Email) ?? throw new Exception("Incorrect Email or Password");

					//if (!user.EmailConfirmed)
					//{
					//    throw new Exception("Email not Confirmed");
					//}
					var isSucceeded = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, true);
					if (isSucceeded.Succeeded)
					{
						_notify.AddSuccessToastMessage("Logged In Successfully");
						if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
						{
							return LocalRedirect(model.ReturnUrl);
						}
						return RedirectToAction("Index", "Home", new { area = "" });
					}
					else if (isSucceeded.IsLockedOut)
					{
						return RedirectToAction(nameof(LockOut));
					}
					else
					{
						_notify.AddErrorToastMessage("Incorrect UserName Or Password");
					}

				}


			}
			catch (Exception ex)
			{
				_notify.AddErrorToastMessage(ex.Message);
				CommonLogger.LogError(ex.Message, ex);

			}
			return RedirectToAction(nameof(Login));
		}
        public async Task<IActionResult> ForgetPassword()
        {
            var model = new ForgetPasswordModel()
            {

            };

            return View(model);
        }
		[HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email) ?? throw new CustomException("Uh-oh! The email address seems to be lost in the digital abyss. Double-check and make sure it's a valid email.");

                if (user.Email != null)
                {
                    string new_password = RandomAlphaNumericHelper.Random(8);

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, new_password).ConfigureAwait(true);
                    if (result.Succeeded)
                    {
                        var htmlString = $"<p>Dear {user.Name} ({user.UserName}),<br/><br/>\r\n&emsp;&emsp; Welcome back! Your password has been successfully reset. Use <b>{new_password}</b> as your new login key to access your account.\r\n<br/>\r\n&emsp;&emsp; Remember to <b>update</b> your password for added security.\r\n<br/><br/>\r\nRegards,<br/>\r\nCommonWorld\r\n</p>";
                        var message = new MessageDto(new string[] { user.Email }, "User Password Reset.", htmlString, null);
                        await _emailService.SendEmailAsync(message).ConfigureAwait(true);
                    }
                }
                else
                {
                    throw new CustomException("Email address of the user is invalid.");
                }
                _notify.AddSuccessToastMessage("Password Reset Email Send Succussfully.");
                return RedirectToAction(nameof(Login));

            }
            catch (Exception ex)
            {
                CommonLogger.LogError(ex.Message, ex);
                _notify.AddErrorToastMessage(ex.Message);
                return View(model);

            }

        }
        public async Task<IActionResult> Register()
        {
            var userModel = new UserViewModel()
            {

            };

            return View(userModel);
        }
        [HttpPost]
		public async Task<IActionResult> Register(UserRegisterViewModel model)
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
					Roles = new List<string>() { DomainModule.Entity.User.TypeGeneral.ToString() }

				};
				var userReponse = await _userService.Create(createDto);

				_notify.AddSuccessToastMessage("Created succesfully. Please Confirm your account");
				return RedirectToAction(nameof(Login));
			}
			catch (Exception ex)
			{
				_notify.AddErrorToastMessage(ex.Message);

			}
			return RedirectToAction(nameof(Login));

		}


		public IActionResult LockOut()
		{
			return View();
		}
		public async Task<IActionResult> LogOut()
		{
			await _signInManager.SignOutAsync();
			_notify.AddSuccessToastMessage("Logged Out Successfully");
			return RedirectToAction(nameof(Login));
		}

		public IActionResult ConfirmEmailPage(string confirmationLink)
		{
			return View(new ConfirmEmailPageModel { ConfirmationLink = confirmationLink });
		}




		public async Task<IActionResult> ConfirmEmail(string email, string token)
		{
			try
			{
				var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(true) ?? throw new UserNotFoundException();
				var isConfirmed = await _userManager.ConfirmEmailAsync(user, token);
				if (isConfirmed.Succeeded)
				{
					_notify.AddSuccessToastMessage("Email Confirmed Successfully");
					return RedirectToAction(nameof(Login));
				}
				else
				{
					_notify.AddSuccessToastMessage("Email Confirmation failed");
				}
			}
			catch (Exception ex)
			{
				CommonLogger.LogError(ex.Message, ex);
				_notify.AddErrorToastMessage(ex.Message);
			}

			return RedirectToAction(nameof(Login));
		}

	}
}
