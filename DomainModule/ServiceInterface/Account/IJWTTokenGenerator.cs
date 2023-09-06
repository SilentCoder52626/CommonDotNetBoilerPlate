using DomainModule.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModule.ServiceInterface.Account
{
	public interface IJWTTokenGenerator
	{
		string GenerateToken(JWTTokenDto tokenDto);
	}
}
