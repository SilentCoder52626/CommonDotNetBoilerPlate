using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            if (code == 404)
            {
                return RedirectToAction(nameof(NotFound));
            }
            return View();
        }
        public IActionResult NotFound()
        {
            return View();
        }
    }
}
