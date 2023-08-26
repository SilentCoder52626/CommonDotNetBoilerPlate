using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModel
{
    public class LoginViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; } = "/Home/Index";
        public IList<AuthenticationScheme> ExternalProviders { get; set; } = new List<AuthenticationScheme>();

    }
}
