using DomainModule.Entity;
using DomainModule.RepositoryInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.ViewModel;

namespace WebApp.Areas.API.Controllers
{
	[Route("api/user")]
	[ApiController]
	public class UserApiController : ControllerBase
	{
		private readonly UserRepositoryInterface _userRepo;

		public UserApiController(UserRepositoryInterface userRepo)
		{
			_userRepo = userRepo;
		}

		[Authorize(Policy = "User-View")]
		[HttpGet("")]
		public async Task<IActionResult> GetUser()
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
			return Ok(userIndexViewModels);
		}

	}
}
