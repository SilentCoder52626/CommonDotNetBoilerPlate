using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModel
{
    public class RoleIndexViewModel
    {
        public long Sno { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class RoleViewModel
    {
        [Required(ErrorMessage ="Role is required")]
        public string RoleName { get; set; }
    }
    public class RoleEditViewModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string Name { get; set; }
    }

 
}
