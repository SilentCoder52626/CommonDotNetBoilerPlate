using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebApp.ViewModel
{

    public class UserIndexViewModel
    {
        public int SN { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
    }

    public class UserViewModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
         ErrorMessage = "Invalid email format")]
        public string EmailAddress { get; set; }
        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid mobile number")]
        public string MobileNumber { get; set; }

        [AllowNull]
        public List<string> Roles { get; set; }

    }

    public class UserEditViewModel
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Full Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
         ErrorMessage = "Invalid email format")]
        public string EmailAddress { get; set; }
        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid mobile number")]
        public string MobileNumber { get; set; }

        [AllowNull]
        public List<string> Roles { get; set; }

    }


    public class UserRegisterViewModel
    {
        [Required(ErrorMessage = "Full Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
         ErrorMessage = "Invalid email format")]
        public string EmailAddress { get; set; }
        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid mobile number")]
        public string MobileNumber { get; set; }

      

    }


}
