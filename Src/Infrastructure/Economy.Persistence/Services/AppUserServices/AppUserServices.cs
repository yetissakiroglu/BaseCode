using Core.Models.Business;
using Core.Models.Dto;
using Economy.Application.Repositories.UserServiceRepositories;
using Economy.Application.Repositories.UserServiceRepositoriesa;
using Economy.Application.Services.AppUserServices;
using Economy.Application.UnitOfWorks;
using Economy.Domain.BaseEntities;
using Economy.Domain.Entites.EntityAppUsers;
using Economy.Domain.Entites.Identities;
using Economy.Infrastructure.DateFormats;
using Economy.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net;

namespace Economy.Persistence.Services.AppUserServices
{
    public class AppUserServices(IAppUserRepository repository, IRefreshTokenRepository refreshTokenRepository, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IOptions<DateFormatConfiguration> dateFormatConfiguration, ITokenService tokenService, IOptions<List<Client>> clients)
     :  IAppUserServices
    {
        private readonly IAppUserRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IRefreshTokenRepository _userRefreshTokenRepository = refreshTokenRepository;
        private readonly UserManager<AppUser> _userManager = userManager;
		private readonly ITokenService _tokenService = tokenService;
		private readonly List<Client> _clients = clients.Value;

		//protected readonly DateFormatConfiguration _dateFormatConfiguration = dateFormatConfiguration.Value;
		//protected readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
		//protected readonly IAuditColumnTransformer _auditColumnTransformer = auditColumnTransformer;




		public ServiceResult<ClientToken> CreateToken(ClientSignIn clientSignIn)
		{
			var client = _clients.SingleOrDefault(s => s.Id == clientSignIn.ClientId && s.Secret == clientSignIn.ClientSecret);

			if (client == null)
			{
				return ServiceResult<ClientToken>.Fail("Client credentials are invalid.", HttpStatusCode.NotFound);
			}

			var token = _tokenService.CreateToken(client);
			return ServiceResult<ClientToken>.Success(token, HttpStatusCode.OK);
		}

		public async Task<ServiceResult<Token>> CreateTokenAsync(SignIn signIn)
		{
			if (signIn == null)
			{
				throw new ArgumentNullException(nameof(signIn));
			}

			var user = await _userManager.FindByEmailAsync(signIn.Email);
			if (user == null)
			{
				return ServiceResult<Token>.Fail("Email or password is invalid.",HttpStatusCode.NotFound);
			}

			if (!await _userManager.CheckPasswordAsync(user, signIn.Password))
			{

				return ServiceResult<Token>.Fail("Email or password is invalid.");
			}

			var token = _tokenService.CreateToken(user);
			var userRefreshToken = await _userRefreshTokenRepository.Table.Where(w => w.UserId == user.Id).SingleOrDefaultAsync();
			if (userRefreshToken == null)
			{
				await _userRefreshTokenRepository.AddAsync(new UserRefreshToken
				{
					UserId = user.Id,
					Token = token.RefreshToken,
					Expiration = token.RefreshTokenExpiration
				});
			}
			else
			{
				userRefreshToken.Token = token.RefreshToken;
				userRefreshToken.Expiration = token.RefreshTokenExpiration;
			}

			await _unitOfWork.CommitAsync();

			return ServiceResult<Token>.Success(token, HttpStatusCode.OK);
		}

		public  async Task<ServiceResult<Token>> RefreshTokenAsync(string refreshToken)
		{
			var entity = await _userRefreshTokenRepository.Table.Where(w => w.Token == refreshToken).SingleOrDefaultAsync();
			if (entity == null)
			{
				return ServiceResult<Token>.Fail("Refresh token not found.", HttpStatusCode.NotFound);
			}

			var user = await _userManager.FindByIdAsync(entity.UserId);
			if (user == null)
			{
				return ServiceResult<Token>.Fail("User not found.", HttpStatusCode.NotFound);
			}

			var token = _tokenService.CreateToken(user);

			entity.Token = token.RefreshToken;
			entity.Expiration = token.RefreshTokenExpiration;

			await _unitOfWork.CommitAsync();

			return ServiceResult<Token>.Success(token, HttpStatusCode.OK);

		}

		public async Task<ServiceResult<string>> RevokeRefreshTokenAsync(string refreshToken)
		{
			var entity = await _userRefreshTokenRepository.Table.Where(w => w.Token == refreshToken).SingleOrDefaultAsync();
			if (entity == null)
			{
				return ServiceResult<string>.Fail("Refresh token not found.", HttpStatusCode.NotExtended);
			}

			_userRefreshTokenRepository.Table.Remove(entity);

			await _unitOfWork.CommitAsync();

			return ServiceResult<string>.Success(refreshToken,HttpStatusCode.OK);
		}










		public async Task DeleteAsync(string? id)
		{

			var entity = await _repository.Table
				.ApplyIsDeletedFalseFilter(isApplyFilter: true)
				.FirstOrDefaultAsync(x => x.Id == id);

			if (entity != null)
			{

				if (entity.IsDefaultAdmin == true)
				{
					throw new Exception($"Unable to delete default admin: {entity.UserName}");
				}


				if (entity is ISoftDelete softDeleteEntity)
				{
					softDeleteEntity.IsDeleted = true;
					await _repository.UpdateAsync(entity);
				}
				else
				{
					await _repository.DeleteAsync(entity);
				}

				await _unitOfWork.CommitAsync();
			}
		}

		public async Task<AppUser?> GetUndeletedUserAsync(string email)
		{
			var user = await _repository.Table
				.Where(x => x.UserName == email)
				.ApplyIsDeletedFalseFilter(true)
				.AsNoTracking()
				.FirstOrDefaultAsync();

			return user;
		}


	}
}
