using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModule.Dto.User
{
  public  class UserDto
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string Type { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public string CurrentSiteDomain { get; set; } 
    }
    public class UserEditDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public List<string> Roles { get; set; } = new List<string>();

    }

    public class UserResponseDto
    {
        public string EmailConfirmationLink { get; set; }
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
		public string Name { get; set; }
		public string UserName { get; set; }
	}
}
