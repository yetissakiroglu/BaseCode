using Economy.Core.Helpers;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.EntityAppContents.AppContents;
using Economy.Panel.Application.Dtos.AppContentDtos;
using Economy.Panel.Application.Extensions;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.Persistence.Extensions;
using FluentValidation;
using System.Net;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppContentService : IPanelAppContentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppContent, int> _entityRepository;
        private readonly IFileImageHelperService _fileImageHelperService;
        private readonly IValidator<AppContentCreateEditDto> _validator;
        public PanelAppContentService(IUnitOfWork unitOfWork, IFileImageHelperService fileImageHelperService, IValidator<AppContentCreateEditDto> validator)
        {
            _unitOfWork = unitOfWork;
            _entityRepository = unitOfWork.HotelEntityRepository<AppContent>();
            _fileImageHelperService = fileImageHelperService;
            _validator = validator;
        }

        public ServiceResult<AppContentDto> CreateContent(AppContentCreateEditDto model)
        {
            if (model == null)
                return ServiceResult<AppContentDto>.Failure(message: "Geçersiz veri gönderildi.");

            if (model.Translations == null || model.Translations.Count == 0)
                return ServiceResult<AppContentDto>.Failure(
                    message: "En az bir dil içeriği eklenmelidir.",
                    statusCode: (int)HttpStatusCode.BadRequest);

            var validationResult = _validator.Validate(model);
            if (!validationResult.IsValid)
                return ServiceResult<AppContentDto>.Failure(
                    message: "Geçersiz giriş verisi.",
                    statusCode: (int)HttpStatusCode.BadRequest,
                    validationErrors: validationResult.ToValidationDictionary());


            var entity = new AppContent();
            ContentMapper.MapToEntity(entity, model);


            _entityRepository.Add(entity);
            _unitOfWork.SaveHotelChanges();

            return ServiceResult<AppContentDto>.Success(
                data: entity.MapToDto(),
                message: "Başarıyla oluşturuldu.",
                statusCode: (int)HttpStatusCode.Created);
        }

        public ServiceResult<AppContentDto> DeleteContent(int id)
        {
            var slide = _entityRepository.GetForEdit(w => w.Id == id && !w.IsDeleted);
            if (slide == null)
            {
                return ServiceResult<AppContentDto>.Failure(message: "data bulunamadı.", statusCode: (int)HttpStatusCode.NotFound);
            }

            slide.IsDeleted = true;
            _entityRepository.Update(slide);
            _unitOfWork.SaveHotelChanges();

            return ServiceResult<AppContentDto>.Success(slide.MapToDto(), "başarıyla silindi.");

        }

        public ServiceResult<AppContentDto> EditContent(AppContentCreateEditDto model)
        {
            if (model == null || model.Id == null)
                return ServiceResult<AppContentDto>.Failure(
                    message: "Geçersiz veri.",
                    statusCode: (int)HttpStatusCode.BadRequest);

            if (model.Translations == null || !model.Translations.Any())
                return ServiceResult<AppContentDto>.Failure(
                    message: "Geçersiz dil verisi.",
                    statusCode: (int)HttpStatusCode.BadRequest);


            var validationResult = _validator.Validate(model);
            if (!validationResult.IsValid)
                if (!validationResult.IsValid)
                    return ServiceResult<AppContentDto>.Failure(
                        message: "Geçersiz giriş verisi.",
                        statusCode: (int)HttpStatusCode.BadRequest,
                        validationErrors: validationResult.ToValidationDictionary());

            var entity = _entityRepository.GetForRead(w => w.Id == model.Id && !w.IsDeleted, w => w.Translations);
            if (entity == null)
                return ServiceResult<AppContentDto>.Failure(
                    message: "Data bulunamadı.",
                    statusCode: (int)HttpStatusCode.BadRequest);


            ContentMapper.MapToEntity(entity, model);


            _entityRepository.Update(entity);
            _unitOfWork.SaveHotelChanges();

            return ServiceResult<AppContentDto>.Success(
                   data: entity.MapToDto(),
                   message: "başarıyla güncellendi.");
        }

        public ServiceResult<List<AppContentDto>> GetAllContent(bool isDeleted)
        {
            var entity = _entityRepository.WhereForRead(x => x.IsDeleted == isDeleted, x => x.Translations).Select(ContentMapper.MapSelectToDto).ToList();
            if (!entity.Any())
            {
                return ServiceResult<List<AppContentDto>>.Empty(
                    message: "Kayıt bulunamadı.",
                    statusCode: (int)HttpStatusCode.NoContent);
            }

            return ServiceResult<List<AppContentDto>>.Success(
                data: entity,
                message: "başarıyla getirildi.");
        }

        public ServiceResult<AppContentDto> GetContent(int id, bool isDeleted)
        {
            var entity = _entityRepository.GetForRead(x => x.Id == id && x.IsDeleted == isDeleted, x => x.Translations);
            if (entity == null)
            {
                return ServiceResult<AppContentDto>.Empty(
                   message: $"ID si {id} olan dil kaydı bulunamadı.",
                   statusCode: (int)HttpStatusCode.NotFound
               );
            }

            return ServiceResult<AppContentDto>.Success(data: entity.MapToDto(), message: "başarıyla getirildi.", statusCode: (int)HttpStatusCode.OK);

        }
    }
}
