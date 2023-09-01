using DomainModule.Dto;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace WebApp.AuthenticationAuthorization
{
    public static class AuthentiationAndAuthorizationConfig
    {

        public static void ConfigureAuthentication(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Events.OnRedirectToAccessDenied = (evnt) =>
                {
                    if (evnt.Request.Path.StartsWithSegments("/api"))
                    {
                        evnt.Response.StatusCode = 403;
                        evnt.Response.ContentType = "application/json";
                        var data = new
                        {
                            Message = "You are not authorized here",
                            Code = StatusCodes.Status401Unauthorized,
                            Status = "401 Unauthorized",
                            Errors = "Unauthorized",
                            LoginRedirectUrl = "/Error/AccessDenied"
                        };
                      evnt.Response.WriteAsJsonAsync(JsonConvert.SerializeObject(data));
                        return Task.CompletedTask;
                    }
                    else
                    {
                        evnt.Response.Redirect("/Error/AccessDenied");
                    }

                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToLogin = (evnt) =>
                {
                    if (evnt.Request.Path.StartsWithSegments("/api"))
                    {
                        evnt.Response.StatusCode = 401;
                        evnt.Response.ContentType = "application/json";
                        var data = new
                        {
                            Message = "You are not authorized here",
                            Code = StatusCodes.Status401Unauthorized,
                            Status = "401 Unauthorized",
                            Errors = "Unauthorized",
                            LoginRedirectUrl = "/Account/Account/Login"
                        };
                        evnt.Response.WriteAsJsonAsync(JsonConvert.SerializeObject(data));
                        return Task.CompletedTask;
                    }
                    else
                    {
                        var returnUrl = evnt.Request.Path;
                        if (string.IsNullOrWhiteSpace(returnUrl) || returnUrl == "/")
                            returnUrl = "/Home/Index";
                        evnt.Response.Redirect("/Account/Account/Login?ReturnUrl=" + returnUrl);

                    }

                    return Task.CompletedTask;
                };


            });
            services.AddAuthorization(options =>
            {
                foreach (var permission in PermissionHelper.GetPermission().Permissions)
                {
                    options.AddPolicy(permission, policy => policy.Requirements.Add(new PermissionRequirement(permission)));
                }
            });
            services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();


        }
    }
}
