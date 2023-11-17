using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WebApp.Controllers
{
	[AllowAnonymous]
	public class ErrorController : Controller
	{
		public IActionResult AccessDenied()
		{
			return View();
		}
		[Route("Error/Error/{code:int}")]
		public IActionResult Error(int code)
		{
			var statusCodeFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

			if (code == 404)
			{
				if (statusCodeFeature.OriginalPath.StartsWith("/api",StringComparison.InvariantCultureIgnoreCase))
				{
					Response.StatusCode = 404;
					Response.ContentType = "application/json";
					var data = new
					{
						Message = "Requested page not found. Please check your link.",
						Code = StatusCodes.Status404NotFound,
						Status = "404 NotFound",
						Errors = "NotFound",
						LoginRedirectUrl = "/Error/NotFound"
					};
					return BadRequest(data);
				}
				else
				{
					return RedirectToAction(nameof(NotFound));
				}
			}
			else
			{
				if (statusCodeFeature.OriginalPath.StartsWith("/api", StringComparison.InvariantCultureIgnoreCase))
				{
					Response.StatusCode = 500;
					Response.ContentType = "application/json";
					var data = new
					{
						Message = "Something went wrong. Please contact adminstrator.",
						Code = StatusCodes.Status500InternalServerError,
						Status = "500 InternalServerError",
						Errors = "InternalServerError",
						LoginRedirectUrl = "/Error/Error"
					};
					return BadRequest(data);
				}
				else
				{
					return View();
				}
			}
		}
		public IActionResult NotFound()
		{
			return View();
		}
	}
}
