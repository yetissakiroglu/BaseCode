using Economy.Core.Business;
using Economy.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Economy.Application.Extensions
{
    public static class IdentityExtension
	{
		public static void AddAuthentication(this IServiceCollection services, TokenOption tokenOption)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidIssuer = tokenOption.Issuer,
					ValidAudience = tokenOption.Audiences.FirstOrDefault(),
					IssuerSigningKey = SignInService.GetSymmetricSecurityKey(tokenOption.SecurityKey),
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				};

				options.Events = new JwtBearerEvents
				{
					OnTokenValidated = ctx => Task.CompletedTask,
					OnAuthenticationFailed = ctx => Task.CompletedTask
				};
			});
		}
	}

}
