using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModel
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage ="Old Password is required")]
        public string OldPasword { get; set; }
        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare("NewPassword",ErrorMessage ="password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
