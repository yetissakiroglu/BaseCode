using Economy.Application.BaseRepositories;
using Economy.Base.Application.BaseRepositories;
using Economy.Base.Application.Dtos.BaseModels;
using Economy.Base.Application.Interfaces;
using Economy.Core.Dtos;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Core.Tools.Models;
using Economy.Domain.Entites.Identities;
using Economy.Domain.Entities.Identity;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories.AppBase.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Economy.Persistence.BaseRepositories
{
    public class AppUserBaseRepository : EfEntityRepositoryBase<AppUser>, IAppUserBaseRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IAppUserTokenBaseRepository _appUserTokenBaseRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AppUserBaseRepository(DefaultDbContext _context, UserManager<AppUser> userManager, ITokenService tokenService, IAppUserTokenBaseRepository appUserTokenBaseRepository, IUnitOfWork unitOfWork) : base(_context)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _appUserTokenBaseRepository = appUserTokenBaseRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseModel<AppUserDto>> CreateUser(AppUserCreateDto userCreateDto)
        {
            var response = new ResponseModel<AppUserDto>();

            var user = new AppUser
            {
                FirstName = userCreateDto.FirstName,
                LastName = userCreateDto.LastName,
                TenantId = userCreateDto.TenantId,
                UserName = userCreateDto.UserName,
                Email = userCreateDto.Email,
                PhoneNumber = userCreateDto.PhoneNumber,
                EmailConfirmed = userCreateDto.EmailConfirmed,
                PhoneNumberConfirmed = userCreateDto.PhoneNumberConfirmed
            };

            var result = await _userManager.CreateAsync(user, userCreateDto.Password);

            var resultDto = new AppUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                IsDefaultAdmin =user.IsDefaultAdmin,
                FirstName = user.FirstName,
                LastName = user.FirstName,
                TenantId = user.TenantId,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed
            };


            if (result.Succeeded)
            {
                response.IsSuccess = true;
                response.Data = resultDto;
                response.Message  = new ResultMessage("Kullanıcı başarıyla oluşturuldu.");
            }
            else
            {
                response.IsSuccess = false;
                response.Message = new ResultMessage(string.Join(" | ", result.Errors.Select(e => e.Description)));

            }

            return response;


        }

        public async Task<ResponseModel<Token>> LoginAsync(SignIn signIn)
        {
            if (signIn == null)
            {
                throw new ArgumentNullException(nameof(signIn), "Giriş bilgileri geçersiz.");
            }

            var user = await _userManager.FindByEmailAsync(signIn.Email);
            if (user == null)
            {
                return ResponseModel<Token>.Fail("Geçersiz e-posta veya şifre.", HttpStatusCode.Unauthorized);
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, signIn.Password);
            if (!isPasswordValid)
            {
                return ResponseModel<Token>.Fail("Geçersiz e-posta veya şifre.", HttpStatusCode.Unauthorized);
            }

            //Token oluşturma işlemi
            var token = _tokenService.CreateToken(user);
            var loginProvider = $"MyApp_{user.TenantId}";

            //Refresh token'ı veritabanında saklama
            var userToken = new AppUserToken
            {
                UserId = user.Id,
                LoginProvider = loginProvider, // Boş geçme
                Name = "Token",          // Refresh token ya da access token türüne göre değişebilir
                Value = token.AccessToken,
                RefreshToken = token.RefreshToken,
                ExpirationDate = token.RefreshTokenExpiration,
                Token = token.AccessToken,
            };


            //Veritabanına kaydet
            _appUserTokenBaseRepository.Add(userToken);
            await _unitOfWork.SaveDefaultChangesAsync();

            return ResponseModel<Token>.Success(token, HttpStatusCode.OK);
        }
    }
}
