using DomainModule.Dto.User;
using DomainModule.Entity;
using DomainModule.ServiceInterface.Account;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Helper;
using WebApp.ViewModel;

namespace WebApp.Areas.API.Controllers
{
	[Route("api/account")]
	[ApiController]
	public class AccountApiController : ControllerBase
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IJWTTokenGenerator _tokenGenerator;
		private readonly IConfiguration _configuration;
		public AccountApiController(UserManager<User> userManager,
			SignInManager<User> signInManager,
			IJWTTokenGenerator tokenGenerator,
			IConfiguration configuration)
		{
			_userManager = userManager;
			_tokenGenerator = tokenGenerator;
			_configuration = configuration;
			_signInManager = signInManager;
		}
		
		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody]LoginViewModel model)
		{
			try
			{

				var user = await _userManager.FindByEmailAsync(model.Email) ?? throw new Exception("Incorrect Email or Password");

				var isSucceeded = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, true);
				if (isSucceeded.Succeeded)
				{
					var tokenDto = new JWTTokenDto()
					{
						UserId = user.Id,
						UserName = user.UserName,
						Email = user.Email,
						JwtKey = _configuration["JWT:Secret"]
					};
					var token = _tokenGenerator.GenerateToken(tokenDto);
					//BackgroundJob.Enqueue(() => Console.WriteLine(token));
					return Ok(token);
				}
				return BadRequest("Incorrect user name or password");

			}
			catch (Exception ex)
			{
				CommonLogger.LogError(ex.Message, ex);
				return BadRequest(ex.Message);
			}


		}
	}
}
