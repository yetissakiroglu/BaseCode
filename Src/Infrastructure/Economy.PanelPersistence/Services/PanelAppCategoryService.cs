using Economy.Core.Helpers;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.EntityCategories;
using Economy.Panel.Application.Dtos.AppCategoryDtos;
using Economy.Panel.Application.Extensions;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.Persistence.Extensions;
using FluentValidation;
using System.Net;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppCategoryService : IPanelAppCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppCategory, int> _entityRepository;
        private readonly IFileImageHelperService _fileImageHelperService;
        private readonly IValidator<AppCategoryCreateEditDto> _validator;

        public PanelAppCategoryService(IUnitOfWork unitOfWork, IFileImageHelperService fileImageHelperService, IValidator<AppCategoryCreateEditDto> validator)
        {
            _unitOfWork = unitOfWork;
            _entityRepository = unitOfWork.EntityRepository<AppCategory>();
            _fileImageHelperService = fileImageHelperService;
            _validator = validator;
        }

        public ServiceResult<AppCategoryDto> CreateCategory(AppCategoryCreateEditDto model)
        {
            if (model == null)
                return ServiceResult<AppCategoryDto>.Failure(message: "Geçersiz veri gönderildi.");

            if (model.Translations == null || model.Translations.Count == 0)
                return ServiceResult<AppCategoryDto>.Failure(
                    message: "En az bir dil içeriği eklenmelidir.",
                    statusCode: (int)HttpStatusCode.BadRequest);

            var validationResult = _validator.Validate(model);
            if (!validationResult.IsValid)
                return ServiceResult<AppCategoryDto>.Failure(
                    message: "Geçersiz giriş verisi.",
                    statusCode: (int)HttpStatusCode.BadRequest,
                    validationErrors: validationResult.ToValidationDictionary());


            var entity = new AppCategory();
            CategoryMapper.MapToEntity(entity, model);


            _entityRepository.Add(entity);
            _unitOfWork.SaveHotelChanges();

            return ServiceResult<AppCategoryDto>.Success(
                data: entity.MapToDto(),
                message: "Başarıyla oluşturuldu.",
                statusCode: (int)HttpStatusCode.Created);
        }

        public ServiceResult<AppCategoryDto> DeleteCategory(int id)
        {
            var slide = _entityRepository.GetForEdit(w => w.Id == id && !w.IsDeleted);
            if (slide == null)
            {
                return ServiceResult<AppCategoryDto>.Failure(message: "Kategori bulunamadı.", statusCode: (int)HttpStatusCode.NotFound);
            }

            slide.IsDeleted = true;
            _entityRepository.Update(slide);
            _unitOfWork.SaveHotelChanges();

            return ServiceResult<AppCategoryDto>.Success(slide.MapToDto(), "başarıyla silindi.");
        }

        public ServiceResult<AppCategoryDto> EditCategory(AppCategoryCreateEditDto model)
        {
            if (model == null || model.Id == null)
                return ServiceResult<AppCategoryDto>.Failure(
                    message: "Geçersiz veri.",
                    statusCode: (int)HttpStatusCode.BadRequest);

            if (model.Translations == null || !model.Translations.Any())
                return ServiceResult<AppCategoryDto>.Failure(
                    message: "Geçersiz dil verisi.",
                    statusCode: (int)HttpStatusCode.BadRequest);


            var validationResult = _validator.Validate(model);
            if (!validationResult.IsValid)
                if (!validationResult.IsValid)
                    return ServiceResult<AppCategoryDto>.Failure(
                        message: "Geçersiz giriş verisi.",
                        statusCode: (int)HttpStatusCode.BadRequest,
                        validationErrors: validationResult.ToValidationDictionary());

            var entity = _entityRepository.GetForRead(w => w.Id == model.Id && !w.IsDeleted, w => w.Translations);
            if (entity == null)
                return ServiceResult<AppCategoryDto>.Failure(
                    message: "Slide bulunamadı.",
                    statusCode: (int)HttpStatusCode.BadRequest);


            CategoryMapper.MapToEntity(entity, model);
          

            _entityRepository.Update(entity);
            _unitOfWork.SaveHotelChanges();

            return ServiceResult<AppCategoryDto>.Success(
                   data: entity.MapToDto(),
                   message: "başarıyla güncellendi.");
        }

        public ServiceResult<List<AppCategoryDto>> GetAllCategories(bool isDeleted)
        {
            var entity = _entityRepository.WhereForRead(x => x.IsDeleted == isDeleted ,x => x.Translations,x=>x.SubCategories).Select(CategoryMapper.MapSelectToDto).ToList();
            if (!entity.Any())
            {
                return ServiceResult<List<AppCategoryDto>>.Empty(
                    message: "Kayıt bulunamadı.",
                    statusCode: (int)HttpStatusCode.NoContent);
            }

            return ServiceResult<List<AppCategoryDto>>.Success(
                data: entity,
                message: "başarıyla getirildi.");
        }

        public ServiceResult<AppCategoryDto> GetCategory(int id, bool isDeleted)
        {
            var entity = _entityRepository.GetForRead(x => x.Id == id && x.IsDeleted == isDeleted, x => x.Translations);
            if (entity == null)
            {
                return ServiceResult<AppCategoryDto>.Empty(
                   message: $"ID si {id} olan dil kaydı bulunamadı.",
                   statusCode: (int)HttpStatusCode.NotFound
               );
            }

            return ServiceResult<AppCategoryDto>.Success(data: entity.MapToDto(), message: "Slide başarıyla getirildi.", statusCode: (int)HttpStatusCode.OK);

        }
    }
}
