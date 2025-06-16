using Economy.Base.Application.Dtos.BaseModels;
using Economy.Core.Dtos;
using Economy.Core.Helpers;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Domain.Entites.EntitySlides;
using Economy.Domain.Entites.Identities;
using Economy.Panel.Application.Dtos.AppSlideDtos;
using Economy.Panel.Application.Interfaces;
using FluentValidation;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppUserService : IPanelAppUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppUser, int> _entityRepository;

        public PanelAppUserService(IUnitOfWork unitOfWork, IEntityRepository<AppUser, int> entityRepository)
        {
            _unitOfWork = unitOfWork;
            _entityRepository = entityRepository;
        }
        public Task<ResponseModel<AppUserDto>> CreateUser(AppUserCreateDto userCreateDto)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppUserDto> DeleteUser(int Id)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppUserDto> EditUser(AppUserEditDto userEditDto)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppUserDto> GetUser(int id, bool isDeleted)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<Token>> LoginAsync(SignIn signIn)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<List<AppUserListDto>> UserList(bool IsDeleted)
        {
            throw new NotImplementedException();
        }

        //private readonly PanelAppUserRepository _panelAppUserRepository;
        //private readonly IUnitOfWork _unitOfWork;

        //public PanelAppUserService(PanelAppUserRepository panelAppUserRepository, IUnitOfWork unitOfWork)
        //{
        //    _panelAppUserRepository = panelAppUserRepository;
        //    _unitOfWork = unitOfWork;
        //}

        //public async Task<ResponseModel<AppUserDto>> CreateUser(AppUserCreateDto userCreateDto)
        //{
        //    return await _panelAppUserRepository.CreateUser(userCreateDto);
        //}

        //public ResponseModel<AppUserDto> DeleteUser(int Id)
        //{
        //    var result = _panelAppUserRepository.GetForEdit(w => w.Id == Id && w.IsDeleted == false);
        //    if (result == null)
        //    {
        //        return new ResponseModel<AppUserDto>
        //        {
        //            IsSuccess = false,
        //            Message = new ResultMessage("Kullanıcı bulunamadı."),
        //            Status = HttpStatusCode.NotFound
        //        };
        //    }
        //    result.IsDeleted = true;
        //    _panelAppUserRepository.Update(result);
        //    _unitOfWork.SaveDefaultChanges();
        //    return new ResponseModel<AppUserDto>
        //    {
        //        IsSuccess = true,
        //        Message = new ResultMessage("Kullanıcı başarıyla silindi."),
        //        Status = HttpStatusCode.OK
        //    };
        //}

        //public ResponseModel<AppUserDto> EditUser(AppUserEditDto userEditDto)
        //{
        //    var user = _panelAppUserRepository.GetForEdit(x => x.Id == userEditDto.Id);

        //    if (user == null)
        //    {
        //        return new ResponseModel<AppUserDto>
        //        {
        //            IsSuccess = false,
        //            Message = new ResultMessage("Kullanıcı bulunamadı."),
        //            Status = HttpStatusCode.NotFound
        //        };
        //    }

        //    var userUpdateModel = new AppUser
        //    {
        //        Id = user.Id,
        //        FirstName = userEditDto.FirstName,
        //        LastName = userEditDto.LastName,
        //        Email = userEditDto.Email,
        //        PhoneNumber = userEditDto.PhoneNumber,
        //        UserName = user.UserName, // Değişmemesi isteniyor
        //        TenantId = user.TenantId  // Değişmemesi isteniyor
        //    };

        //    _panelAppUserRepository.Update(userUpdateModel);
        //    _unitOfWork.SaveDefaultChanges();
        //    return new ResponseModel<AppUserDto>
        //    {
        //        IsSuccess = false,
        //        Message = new ResultMessage("Kullanıcı güncellenemedi."),
        //        Status = HttpStatusCode.BadRequest,
        //        Data = new AppUserDto
        //        {
        //            Id = userUpdateModel.Id,
        //            FirstName = userUpdateModel.FirstName,
        //            LastName = userUpdateModel.LastName,
        //            Email = userUpdateModel.Email,
        //            PhoneNumber = userUpdateModel.PhoneNumber,
        //            UserName = user.UserName, // Değişmemesi isteniyor
        //            TenantId = user.TenantId  // Değişmemesi isteniyor
        //        },
        //    };

        //}

        //public ResponseModel<AppUserDto> GetUser(int id, bool isDeleted)
        //{
        //    var result = _panelAppUserRepository.GetForRead(w => w.Id == id && w.IsDeleted == isDeleted);
        //    var resultModel = new ResponseModel<AppUserDto>();
        //    return new ResponseModel<AppUserDto>()
        //    {
        //        IsSuccess = true,
        //        Data = new AppUserDto
        //        {
        //            Id = result.Id,
        //            FirstName = result.FirstName,
        //            LastName = result.LastName,
        //            Email = result.Email,
        //            PhoneNumber = result.PhoneNumber,
        //            UserName = result.UserName,
        //            TenantId = result.TenantId
        //        },
        //        Message = new ResultMessage("Kullanıcı Başarıyla Getirildi"),
        //        Notification = Core.Enums.NotificationType.Success,
        //        Status = HttpStatusCode.OK
        //    }; 
        //}

        //public async Task<ResponseModel<Token>> LoginAsync(SignIn signIn)
        //{
        //    return await _panelAppUserRepository.LoginAsync(signIn);
        //}
        //public ResponseModel<List<AppUserListDto>> UserList(bool IsDeleted)
        //{
        //    var result = _panelAppUserRepository.WhereForRead(w => w.IsDeleted == IsDeleted);

        //    return new ResponseModel<List<AppUserListDto>>()
        //    {
        //        IsSuccess = true,
        //        Data = result.Select(s => new AppUserListDto
        //        {
        //            Id = s.Id,
        //            FirstName = s.FirstName,
        //            LastName = s.LastName,
        //            Email = s.Email,
        //            PhoneNumber = s.PhoneNumber,
        //            IsDefaultAdmin = s.IsDefaultAdmin,
        //            TenantId = s.TenantId,
        //            UserName = s.UserName
        //        }).ToList(),
        //        Message = new ResultMessage("Kullanıcı Listesi Başarıyla Getirildi"),
        //        Notification = Core.Enums.NotificationType.Success,
        //        Status = HttpStatusCode.OK
        //    };

        //}


        //public async Task<ResponseModel<AppUserDto>> CreateUser(AppUserCreateDto userCreateDto)
        //{
        //    var response = new ResponseModel<AppUserDto>();

        //    var user = new AppUser
        //    {
        //        FirstName = userCreateDto.FirstName,
        //        LastName = userCreateDto.LastName,
        //        TenantId = userCreateDto.TenantId,
        //        UserName = userCreateDto.UserName,
        //        Email = userCreateDto.Email,
        //        PhoneNumber = userCreateDto.PhoneNumber,
        //        EmailConfirmed = userCreateDto.EmailConfirmed,
        //        PhoneNumberConfirmed = userCreateDto.PhoneNumberConfirmed
        //    };

        //    var result = await _userManager.CreateAsync(user, userCreateDto.Password);

        //    var resultDto = new AppUserDto
        //    {
        //        Id = user.Id,
        //        UserName = user.UserName,
        //        IsDefaultAdmin = user.IsDefaultAdmin,
        //        FirstName = user.FirstName,
        //        LastName = user.FirstName,
        //        TenantId = user.TenantId,
        //        Email = user.Email,
        //        PhoneNumber = user.PhoneNumber,
        //        EmailConfirmed = user.EmailConfirmed,
        //        PhoneNumberConfirmed = user.PhoneNumberConfirmed
        //    };


        //    if (result.Succeeded)
        //    {
        //        response.IsSuccess = true;
        //        response.Data = resultDto;
        //        response.Message = new ResultMessage("Kullanıcı başarıyla oluşturuldu.");
        //    }
        //    else
        //    {
        //        response.IsSuccess = false;
        //        response.Message = new ResultMessage(string.Join(" | ", result.Errors.Select(e => e.Description)));

        //    }

        //    return response;


        //}

        //public async Task<ResponseModel<Token>> LoginAsync(SignIn signIn)
        //{
        //    if (signIn == null)
        //    {
        //        throw new ArgumentNullException(nameof(signIn), "Giriş bilgileri geçersiz.");
        //    }

        //    var user = await _userManager.FindByEmailAsync(signIn.Email);
        //    if (user == null)
        //    {
        //        return ResponseModel<Token>.Fail("Geçersiz e-posta veya şifre.", HttpStatusCode.Unauthorized);
        //    }

        //    var isPasswordValid = await _userManager.CheckPasswordAsync(user, signIn.Password);
        //    if (!isPasswordValid)
        //    {
        //        return ResponseModel<Token>.Fail("Geçersiz e-posta veya şifre.", HttpStatusCode.Unauthorized);
        //    }

        //    //Token oluşturma işlemi
        //    var token = _tokenService.CreateToken(user);
        //    var loginProvider = $"MyApp_{user.TenantId}";

        //    //Refresh token'ı veritabanında saklama
        //    var userToken = new AppUserToken
        //    {
        //        UserId = user.Id,
        //        LoginProvider = loginProvider, // Boş geçme
        //        Name = "Token",          // Refresh token ya da access token türüne göre değişebilir
        //        Value = token.AccessToken,
        //        RefreshToken = token.RefreshToken,
        //        ExpirationDate = token.RefreshTokenExpiration,
        //        Token = token.AccessToken,
        //    };


        //    //Veritabanına kaydet
        //    _appUserTokenBaseRepository.Add(userToken);
        //    await _unitOfWork.SaveDefaultChangesAsync();

        //    return ResponseModel<Token>.Success(token, HttpStatusCode.OK);
        //}

    }
}
