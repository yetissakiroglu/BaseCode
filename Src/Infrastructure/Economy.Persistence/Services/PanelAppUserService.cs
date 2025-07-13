using Economy.Base.Application.Dtos.BaseModels;
using Economy.Core.Dtos;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Core.Tools.Models;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.AppEntities;
using Economy.Domain.Entites.Identities;
using Economy.Domain.Entities.Identity;
using Economy.Panel.Application.Dtos.AppLanguageDtos;
using Economy.Panel.Application.Extensions;
using Economy.Panel.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppUserService : IPanelAppUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppUserToken, int> _appUserTokenRepository;
        private readonly IEntityRepository<AppUser, int> _appUserRepository;
        private readonly IValidator<AppUserCreateDto> _validator;
        private readonly IEntityRepository<App, int> _appRepository;

        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        public PanelAppUserService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, ITokenService tokenService, IValidator<AppUserCreateDto> validator)
        {
            _unitOfWork = unitOfWork;
            _appUserTokenRepository = unitOfWork.DefaultEntityRepository<AppUserToken>();
            _appUserRepository = unitOfWork.DefaultEntityRepository<AppUser>();
            _appRepository = unitOfWork.DefaultEntityRepository<App>();
            _userManager = userManager;
            _tokenService = tokenService;
            _validator = validator;
        }
        public async Task<ServiceResult<AppUserDto>> CreateUser(AppUserCreateDto model)
        {
            if (model == null)
            {
                return ServiceResult<AppUserDto>.Failure(
                    message: "Geçersiz veri gönderildi.",
                    statusCode: (int)HttpStatusCode.BadRequest);
            }

            // FluentValidation kontrolü
            var validationResult = _validator.Validate(model);
            if (!validationResult.IsValid)
            {
                return ServiceResult<AppUserDto>.Failure(
                    message: "Geçersiz giriş verisi.",
                    statusCode: (int)HttpStatusCode.BadRequest,
                    validationErrors: validationResult.ToValidationDictionary());
            }

            // Kullanıcı oluştur
            var user = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                TenantId = model.TenantId,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                EmailConfirmed = model.EmailConfirmed,
                PhoneNumberConfirmed = model.PhoneNumberConfirmed
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {

                return ServiceResult<AppUserDto>.Failure(
                    message: "Kullanıcı oluşturulamadı.",
                    statusCode: (int)HttpStatusCode.BadRequest,
                    validationErrors: result.Errors.ToValidationDictionary());
            }

            // Mapping - DTO oluştur
            var resultDto = new AppUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                IsDefaultAdmin = user.IsDefaultAdmin,
                FirstName = user.FirstName,
                LastName = user.LastName,
                TenantId = user.TenantId,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed
            };

            return ServiceResult<AppUserDto>.Success(
                data: resultDto,
                message: "Kullanıcı başarıyla oluşturuldu.",
                statusCode: (int)HttpStatusCode.Created);
        }

        public ResponseModel<AppUserDto> DeleteUser(int Id)
        {
            var result = _appUserRepository.GetForEdit(w => w.Id == Id && w.IsDeleted == false);
            if (result == null)
            {
                return new ResponseModel<AppUserDto>
                {
                    IsSuccess = false,
                    Message = new ResultMessage("Kullanıcı bulunamadı."),
                    Status = HttpStatusCode.NotFound
                };
            }
            result.IsDeleted = true;
            _appUserRepository.Update(result);
            _unitOfWork.SaveDefaultChanges();
            return new ResponseModel<AppUserDto>
            {
                IsSuccess = true,
                Message = new ResultMessage("Kullanıcı başarıyla silindi."),
                Status = HttpStatusCode.OK
            };
        }

        public ResponseModel<AppUserDto> EditUser(AppUserEditDto userEditDto)
        {
            var user = _appUserRepository.GetForEdit(x => x.Id == userEditDto.Id);

            if (user == null)
            {
                return new ResponseModel<AppUserDto>
                {
                    IsSuccess = false,
                    Message = new ResultMessage("Kullanıcı bulunamadı."),
                    Status = HttpStatusCode.NotFound
                };
            }

            var userUpdateModel = new AppUser
            {
                Id = user.Id,
                FirstName = userEditDto.FirstName,
                LastName = userEditDto.LastName,
                Email = userEditDto.Email,
                PhoneNumber = userEditDto.PhoneNumber,
                UserName = user.UserName, // Değişmemesi isteniyor
                TenantId = user.TenantId  // Değişmemesi isteniyor
            };

            _appUserRepository.Update(userUpdateModel);
            _unitOfWork.SaveDefaultChanges();
            return new ResponseModel<AppUserDto>
            {
                IsSuccess = false,
                Message = new ResultMessage("Kullanıcı güncellenemedi."),
                Status = HttpStatusCode.BadRequest,
                Data = new AppUserDto
                {
                    Id = userUpdateModel.Id,
                    FirstName = userUpdateModel.FirstName,
                    LastName = userUpdateModel.LastName,
                    Email = userUpdateModel.Email,
                    PhoneNumber = userUpdateModel.PhoneNumber,
                    UserName = user.UserName, // Değişmemesi isteniyor
                    TenantId = user.TenantId  // Değişmemesi isteniyor
                },
            };
        }

        public ServiceResult<AppUserDto> GetUser(int id, bool isDeleted)
        {
            var entity = _appUserRepository.GetForRead(w => w.Id == id && w.IsDeleted == isDeleted);
            if (entity is null)
            {
                return ServiceResult<AppUserDto>.Empty(
                    message: $"ID'si {id} olan kayıt bulunamadı.",
                    statusCode: (int)HttpStatusCode.NotFound
                );
            }

            var dto = new AppUserDto
            {
                Id = entity.Id,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Email = entity.Email,
                PhoneNumber = entity.PhoneNumber,
                UserName = entity.UserName,
                TenantId = entity.TenantId,
                IsDefaultAdmin = entity.IsDefaultAdmin,
                JobTitle = entity.JobTitle,
                PhotoUrl = entity.PhotoUrl,
            };

            return ServiceResult<AppUserDto>.Success(
                data: dto,
                message: "Kaydı başarıyla getirildi.",
                statusCode: (int)HttpStatusCode.OK
            );








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
            _appUserTokenRepository.Add(userToken);
            await _unitOfWork.SaveDefaultChangesAsync();

            return ResponseModel<Token>.Success(token, HttpStatusCode.OK);
        }

        public ServiceResult<List<AppUserListDto>> UserList(bool IsDeleted)
        {
            var result = _appUserRepository.WhereForRead(w => w.IsDeleted == IsDeleted);

            if (!result.Any())
            {
                return ServiceResult<List<AppUserListDto>>.Empty(
                    message: "Kayıt bulunamadı.",
                    statusCode: (int)HttpStatusCode.NoContent);
            }

            var entity = result.Select(s => new AppUserListDto
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber,
                IsDefaultAdmin = s.IsDefaultAdmin,
                TenantId = s.TenantId,
                UserName = s.UserName
            }).ToList();

            // Uygulama bilgilerini doldur
            foreach (var item in entity)
            {
                item.TenantName = _appRepository.GetForRead(x => x.Id == item.TenantId && x.IsDeleted == false).HotelName;
            }


            return ServiceResult<List<AppUserListDto>>.Success(
             data: entity,
             message: "başarıyla getirildi.");


        }
    }
}
