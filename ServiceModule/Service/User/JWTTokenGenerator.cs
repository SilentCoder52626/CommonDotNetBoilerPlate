using DomainModule.Dto.User;
using DomainModule.ServiceInterface.Account;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModule.Service
{
	public class JWTTokenGenerator : IJWTTokenGenerator
	{
		public string GenerateToken(JWTTokenDto dto)
		{
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(dto.JwtKey));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
			var claims = new List<Claim>()
			{
				   new Claim(JwtRegisteredClaimNames.Sub, dto.UserId.ToString()),
				new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
				new Claim(JwtRegisteredClaimNames.Exp,
					new DateTimeOffset(DateTime.Now.AddMinutes(15)).ToUnixTimeSeconds().ToString()),
				new Claim(ClaimTypes.NameIdentifier, dto.UserId),
				new Claim(ClaimTypes.Email, dto.Email),
			};
			var token = new JwtSecurityToken(null,
			 null,
			  claims,
			  expires: DateTime.Now.AddMinutes(15),
			  signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
