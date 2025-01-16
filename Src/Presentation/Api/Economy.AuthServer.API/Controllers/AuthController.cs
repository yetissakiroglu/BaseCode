using Core.Models.Dto;
using Economy.Application.Services.AppUserServices;
using Microsoft.AspNetCore.Mvc;

namespace Economy.AuthServer.API.Controllers
{


    public class AuthController(IAppUserServices authService) : BaseController
	{
		private readonly IAppUserServices _authService = authService;

        [HttpPost]
		public async Task<IActionResult> CreateToken(SignIn signIn)
		{

			var result = await _authService.CreateTokenAsync(signIn);

			return CreateResult(result);
		}

		[HttpPost]
		public IActionResult CreateTokenByClient(ClientSignIn signIn)
		{
			var result = _authService.CreateToken(signIn);

			return CreateResult(result);
		}

		[HttpPost]
		public async Task<IActionResult> RevokeRefreshToken(string refreshToken)
		{
			var result = await _authService.RevokeRefreshTokenAsync(refreshToken);

			return CreateResult(result);
		}

		[HttpPost]
		public async Task<IActionResult> RefreshToken(string refreshToken)
		{
			var result = await _authService.RefreshTokenAsync(refreshToken);

			return CreateResult(result);
		}
	}

}
