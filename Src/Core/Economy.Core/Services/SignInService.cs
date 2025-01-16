using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Economy.Core.Services
{
	public static class SignInService
	{
		public static SecurityKey GetSymmetricSecurityKey(string securityKey)
		{
			return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey));
		}
	}
}
