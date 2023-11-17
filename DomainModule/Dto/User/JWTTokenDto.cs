using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.Dto.User
{
	public class JWTTokenDto
	{
		public string? UserName { get; set; }
		public string? UserId { get; set; }
		public string? Email { get; set; }
		public string? JwtKey { get; set; }
	}				
}
