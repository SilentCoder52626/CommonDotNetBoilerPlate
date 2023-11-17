using Microsoft.AspNetCore.Authorization;

namespace WebApp.AuthenticationAuthorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
        public string Permission { get; set; }
    }
}
