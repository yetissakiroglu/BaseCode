using Economy.Application.BaseRepositories;
using Economy.Application.Interfaces.AppUserServices;
using Economy.Base.Application.BaseRepositories;
using Economy.Core.Dtos;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
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

            //Refresh token'ı veritabanında saklama
            var userToken = new AppUserToken
            {
                UserId = user.Id,
                Token = token.AccessToken,
                RefreshToken = token.RefreshToken,
                ExpirationDate = token.RefreshTokenExpiration
            };


            //Veritabanına kaydet
            _appUserTokenBaseRepository.Add(userToken);
            await _unitOfWork.SaveChangesAsync();

            return ResponseModel<Token>.Success(token, HttpStatusCode.OK);
        }
    }
}
