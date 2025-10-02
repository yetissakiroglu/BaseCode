using Economy.Application.TenantUI.Dtos.AppSlideDtos;
using Economy.Application.TenantUI.Interfaces;
using Economy.Core.Helpers;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.TenantEntity.EntityAppSlides;
using Economy.Panel.Application.Extensions;
using Economy.Panel.Persistence.Extensions;
using FluentValidation;
using System.Net;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppSlideService : IPanelAppSlideService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppSlide, int> _entityRepository;
        private readonly IFileImageHelperService _fileImageHelperService;
        private readonly IValidator<AppSlideCreateEditDto> _validator;
        public PanelAppSlideService(IUnitOfWork unitOfWork, IFileImageHelperService fileImageHelperService, IValidator<AppSlideCreateEditDto> validator)
        {
            _unitOfWork = unitOfWork;
            _entityRepository = unitOfWork.HotelEntityRepository<AppSlide>();
            _fileImageHelperService = fileImageHelperService;
            _validator = validator;
        }
        public ServiceResult<AppSlideDto> CreateSlide(AppSlideCreateEditDto model)
        {
            if (model == null)
                return ServiceResult<AppSlideDto>.Failure(message: "Geçersiz veri gönderildi.");

            if (model.Translations == null || model.Translations.Count == 0)
                return ServiceResult<AppSlideDto>.Failure(
                    message: "En az bir dil içeriği eklenmelidir.",
                    statusCode: (int)HttpStatusCode.BadRequest);

            var validationResult = _validator.Validate(model);
            if (!validationResult.IsValid)
                return ServiceResult<AppSlideDto>.Failure(
                    message: "Geçersiz giriş verisi.",
                    statusCode: (int)HttpStatusCode.BadRequest,
                    validationErrors: validationResult.ToValidationDictionary());


            var entity = new AppSlide();
            SlideMapper.MapToEntity(entity, model);
            //todo dikkat
            //if (model.ThumbnailBase64 is not null)
            //{
            //    var webImage = _fileImageHelperService.UploadBase64(model.ThumbnailBase64, new List<string> { "updates", "slider" });
            //    entity.ThumbnailBase64 = webImage.Data.MediaFullURL;
            //}

            //if (model.ThumbnailMobilBase64 is not null)
            //{
            //    var mobilImage = _fileImageHelperService.UploadBase64(model.ThumbnailMobilBase64, new List<string> { "updates", "slider" });
            //    entity.ThumbnailMobilBase64 = mobilImage.Data.MediaFullURL;
            //}

            _entityRepository.Add(entity);
            _unitOfWork.SaveHotelChanges();

            return ServiceResult<AppSlideDto>.Success(
                data: entity.MapToDto(),
                message: "Slide başarıyla oluşturuldu.",
                statusCode: (int)HttpStatusCode.Created);

        }
        public ServiceResult<AppSlideDto> DeleteSlide(int id)
        {
            var slide = _entityRepository.GetForEdit(w => w.Id == id && !w.IsDeleted);
            if (slide == null)
            {
                return ServiceResult<AppSlideDto>.Failure(message: "Slide bulunamadı.", statusCode: (int)HttpStatusCode.NotFound);
            }

            slide.IsDeleted = true;
            _entityRepository.Update(slide);
            _unitOfWork.SaveHotelChanges();

            return ServiceResult<AppSlideDto>.Success(slide.MapToDto(), "Slide başarıyla silindi.");
        }
        public ServiceResult<AppSlideDto> EditSlide(AppSlideCreateEditDto model)
        {
            if (model == null || model.Id == null)
                return ServiceResult<AppSlideDto>.Failure(
                    message: "Geçersiz veri.",
                    statusCode: (int)HttpStatusCode.BadRequest);

            if (model.Translations == null || !model.Translations.Any())
                return ServiceResult<AppSlideDto>.Failure(
                    message: "Geçersiz slide dil verisi.",
                    statusCode: (int)HttpStatusCode.BadRequest);


            var validationResult = _validator.Validate(model);
            if (!validationResult.IsValid)
                if (!validationResult.IsValid)
                return ServiceResult<AppSlideDto>.Failure(
                    message: "Geçersiz giriş verisi.",
                    statusCode: (int)HttpStatusCode.BadRequest,
                    validationErrors: validationResult.ToValidationDictionary());

            var entity = _entityRepository.GetForRead(w => w.Id == model.Id && !w.IsDeleted, w => w.Translations);
            if (entity == null)
                return ServiceResult<AppSlideDto>.Failure(
                    message: "Slide bulunamadı.",
                    statusCode: (int)HttpStatusCode.BadRequest);


           SlideMapper.MapToEntity(entity, model);

            //todo: image
            //if (model.ThumbnailBase64 is not null)
            //{
            //    var webImage = _fileImageHelperService.UploadBase64(model.ThumbnailBase64, new List<string> { "updates", "slider" });
            //    entity.ThumbnailBase64 = webImage.Data.MediaFullURL;
            //}

            //if (model.ThumbnailMobilBase64 is not null)
            //{
            //    var mobilImage = _fileImageHelperService.UploadBase64(model.ThumbnailMobilBase64, new List<string> { "updates", "slider" });
            //    entity.ThumbnailMobilBase64 = mobilImage.Data.MediaFullURL;
            //}


            _entityRepository.Update(entity);
            _unitOfWork.SaveHotelChanges();

            return ServiceResult<AppSlideDto>.Success(
                   data: entity.MapToDto(),
                   message: "Slide başarıyla güncellendi.");
        }
        public ServiceResult<List<AppSlideDto>> GetAllSlide(bool isDeleted)
        {
            var slides = _entityRepository.WhereForRead(x => x.IsDeleted == isDeleted, x => x.Translations).Select(SlideMapper.MapSelectToDto).ToList();
            if (!slides.Any())
            {
                return ServiceResult<List<AppSlideDto>>.Empty(
                    message: "Kayıt bulunamadı.",
                    statusCode: (int)HttpStatusCode.NoContent);
            }

            return ServiceResult<List<AppSlideDto>>.Success(
                data: slides,
                message: "Slaytlar başarıyla getirildi.");
        }
        public ServiceResult<AppSlideDto> GetSlide(int id, bool isDeleted)
        {

            var slide = _entityRepository.GetForRead(x => x.Id == id && x.IsDeleted == isDeleted, x => x.Translations);
            if (slide == null)
            {
                return ServiceResult<AppSlideDto>.Empty(
                   message: $"ID si {id} olan dil kaydı bulunamadı.",
                   statusCode: (int)HttpStatusCode.NotFound
               );
            }

            return ServiceResult<AppSlideDto>.Success(data: slide.MapToDto(), message: "Slide başarıyla getirildi.", statusCode: (int)HttpStatusCode.OK);
        }
    }
}
