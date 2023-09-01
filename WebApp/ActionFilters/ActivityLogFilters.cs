using DomainModule.Dto.ActivityLog;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using DomainModule.ServiceInterface.Account;
using Newtonsoft.Json.Linq;
using NLog.Web.LayoutRenderers;
using WebApp.ViewModel;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace WebApp.ActionFilters
{
    public class ActivityLogFilters : ActionFilterAttribute
    {
        private readonly IActivityLogService _activityService;

        public ActivityLogFilters(IActivityLogService activityService)
        {
            _activityService = activityService;
        }


        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controllerName = ((ControllerBase)context.Controller)
               .ControllerContext.ActionDescriptor.ControllerName;

            var actionName = ((ControllerBase)context.Controller)
                 .ControllerContext.ActionDescriptor.ActionName;

            var actionDescriptorRouteValues = ((ControllerBase)context.Controller)
               .ControllerContext.ActionDescriptor.RouteValues;
            var Area = "";
            if (actionDescriptorRouteValues.ContainsKey("area"))
            {
                var area = actionDescriptorRouteValues["area"];
                if (area != null)
                {
                    Area = Convert.ToString(area);
                }
            }

            var session = context.HttpContext.Session.Id;
            var statusCode = context.HttpContext.Response.StatusCode.ToString();
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            var queryString = context.HttpContext.Request.QueryString.Value;
            var data = "";
            var arguments = context.ActionArguments;
            if (arguments.Any())
            {
                var argumentsData = new List<KeyValuePair<string, object>>();
                foreach (var argument in arguments)
                {
                    var modelName = (argument.Value).GetType().Name;
                    if(modelName == "LoginViewModel")
                    {
                        var modelValue = (LoginViewModel)argument.Value;
                        argumentsData.Add(new KeyValuePair<string, object>(argument.Key, new LoginViewModel() { Email = modelValue.Email, ExternalProviders = modelValue.ExternalProviders, RememberMe = modelValue.RememberMe, ReturnUrl = modelValue.ReturnUrl, Password = PasswordHasher(modelValue.Password) }));
                    }
                    else
                    {
                        argumentsData.Add(argument);
                    }

                }
                data = JsonConvert.SerializeObject(argumentsData);
            }
            var browser = context.HttpContext.Request.Headers["user-agent"].ToString();
            var pageAccessed = Convert.ToString(context.HttpContext.Request.Path);
            var header = context.HttpContext.Request.GetTypedHeaders();
            var userId = "";
            var userName = "Anonymous";
            var user = context.HttpContext.User;
            if (user.Claims.Count() > 0)
            {
                userId = user.FindFirst(ClaimTypes.NameIdentifier).Value;
                userName = user.FindFirst(ClaimTypes.Name).Value;
            }
            Uri uriReferer = header.Referer;
            var uriRef = "";
            if (uriReferer != null)
            {
                uriRef = header.Referer.AbsoluteUri;
            }

            var activityDto = new ActivityLogDto()
            {
                ActionName = actionName,
                ActionOn = DateTime.Now,
                Area = Area,
                IpAddress = ipAddress,
                PageAccessed = pageAccessed,
                Browser = browser,
                ControllerName = controllerName,
                Data = data,
                QueryString= queryString,
                SessionId = session,
                Status = statusCode,
                UrlReferrer = uriRef,
                UserId = userId,
                UserName = userName
            };
            await _activityService.Create(activityDto);
            await next();
        }


        private string PasswordHasher(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes

            // derive a 256-bit subkey (use HMACSHA256 with 100,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password!,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

    }
}
