using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Domain.Entites.EntitySlides;
using Economy.Panel.Application.Dtos.AppSlideDtos;
using Economy.Panel.Application.Dtos.AppSlideDtos.SlideTranslationDtos;
using Economy.Panel.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppSlideService : IPanelAppSlideService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppSlide, int> _entityRepository;

        public PanelAppSlideService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _entityRepository = unitOfWork.EntityRepository<AppSlide>();
        }

        public ResponseModel<AppSlideDto> CreateSlide(AppSlideCreateDto model)
        {
            var entity = new AppSlide
            {
                Sequence = model.Sequence,
                ThumbnailBase64 = model.ThumbnailBase64,
                ThumbnailMobilBase64 = model.ThumbnailMobilBase64,
                Translations = model.Translations.Select(t => new AppSlideTranslation
                {
                    AppLanguageId = t.AppLanguageId,
                    Title = t.Title,
                    Content = t.Content,
                    IsExternal = t.IsExternal,
                    ButtonText = t.ButtonText,
                    ButtonUrl = t.ButtonUrl,
                    ButtonIcon = t.ButtonIcon
                }).ToList()
            };

            _entityRepository.Add(entity);
            _unitOfWork.SaveHotelChanges();

            var dto = MapToDto(entity);
            return ResponseModel<AppSlideDto>.Success(dto, HttpStatusCode.Created);
        }

        public ResponseModel<AppSlideDto> EditSlide(AppSlideEditDto model)
        {
            var entity = _entityRepository.GetForReadFunc(x => x.Id == model.Id, x => x.Include(y => y.Translations));
            if (entity == null)
                return ResponseModel<AppSlideDto>.Fail("Kayıt bulunamadı", HttpStatusCode.NotFound);

            entity.Sequence = model.Sequence;
            entity.ThumbnailBase64 = model.ThumbnailBase64;
            entity.ThumbnailMobilBase64 = model.ThumbnailMobilBase64;

            foreach (var transDto in model.Translations)
            {
                var translation = entity.Translations.FirstOrDefault(t => t.AppLanguageId == transDto.AppLanguageId);
                if (translation != null)
                {
                    translation.Title = transDto.Title;
                    translation.Content = transDto.Content;
                    translation.IsExternal = transDto.IsExternal;
                    translation.ButtonText = transDto.ButtonText;
                    translation.ButtonUrl = transDto.ButtonUrl;
                    translation.ButtonIcon = transDto.ButtonIcon;
                }
                else
                {
                    entity.Translations.Add(new AppSlideTranslation
                    {
                        AppLanguageId = transDto.AppLanguageId,
                        Title = transDto.Title,
                        Content = transDto.Content,
                        IsExternal = transDto.IsExternal,
                        ButtonText = transDto.ButtonText,
                        ButtonUrl = transDto.ButtonUrl,
                        ButtonIcon = transDto.ButtonIcon
                    });
                }
            }

            _entityRepository.Update(entity);
            _unitOfWork.SaveHotelChanges();

            var dto = MapToDto(entity);
            return ResponseModel<AppSlideDto>.Success(dto, HttpStatusCode.OK);
        }

        public ResponseModel<AppSlideDto> DeleteSlide(int id)
        {
            var entity = _entityRepository.GetForEdit(x=>x.Id== id);
            if (entity == null)
                return ResponseModel<AppSlideDto>.Fail("Kayıt bulunamadı", HttpStatusCode.NotFound);

            entity.IsDeleted = true;
            _entityRepository.Update(entity);
            _unitOfWork.SaveHotelChanges();

            var dto = MapToDto(entity);
            return ResponseModel<AppSlideDto>.Success(dto, HttpStatusCode.OK);
        }

        public ResponseModel<IEnumerable<AppSlideDto>> GetAllSlide(bool isDeleted)
        {
            var slides = _entityRepository
                .WhereForReadFunc(w => w.IsDeleted == isDeleted, x => x.Include(y => y.Translations))
                .ToList();

            var slideDtos = slides.Select(MapToDto).ToList();

            return ResponseModel<IEnumerable<AppSlideDto>>.Success(slideDtos, HttpStatusCode.OK);
        }

        public ResponseModel<AppSlideDto> GetSlide(int id, bool isDeleted)
        {
            var slide = _entityRepository
                .GetForReadFunc(x => x.Id == id && x.IsDeleted == isDeleted, x => x.Include(y => y.Translations));

            if (slide == null)
                return ResponseModel<AppSlideDto>.Fail("Kayıt bulunamadı", HttpStatusCode.NotFound);

            var dto = MapToDto(slide);
            return ResponseModel<AppSlideDto>.Success(dto, HttpStatusCode.OK);
        }

        private AppSlideDto MapToDto(AppSlide slide)
        {
            return new AppSlideDto
            {
                Id = slide.Id,
                Sequence = slide.Sequence,
                ThumbnailBase64 = slide.ThumbnailBase64,
                ThumbnailMobilBase64 = slide.ThumbnailMobilBase64,
                Translations = slide.Translations?.Select(t => new AppSlideLanguageDto
                {
                    Id = t.Id,
                    AppSlideId = t.AppSlideId,
                    AppLanguageId = t.AppLanguageId,
                    Title = t.Title,
                    Content = t.Content,
                    IsExternal = t.IsExternal,
                    ButtonText = t.ButtonText,
                    ButtonUrl = t.ButtonUrl,
                    ButtonIcon = t.ButtonIcon
                }).ToList()
            };
        }
    }
}
