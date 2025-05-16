using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Panel.Application.Dtos.AppSettingWhatsappLineDtos;
using Economy.Panel.Application.Interfaces;
using System.Net;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppSettingWhatsappLineService : IPanelAppSettingWhatsappLineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppSettingWhatsappLine, int> _entityRepository;

        public PanelAppSettingWhatsappLineService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _entityRepository = unitOfWork.EntityRepository<AppSettingWhatsappLine>();
        }

        public ResponseModel<AppSettingWhatsappLineDto> CreateAppSettingWhatsappLine(AppSettingWhatsappLineCreateDto model)
        {
            var result = _entityRepository.GetForEdit(w => w.CountryCode == model.CountryCode && w.Number == model.Number);
            if (result != null)
            {
                return ResponseModel<AppSettingWhatsappLineDto>.Fail("Bu kayıt zaten mevcut.", HttpStatusCode.Conflict);
            }
            var newModel = new AppSettingWhatsappLine
            {
                CountryCode = model.CountryCode,
                Number = model.Number,
                IsPrimary = model.IsPrimary
            };
            _entityRepository.Add(newModel);
            _unitOfWork.SaveHotelChanges();
            return ResponseModel<AppSettingWhatsappLineDto>.Success(new AppSettingWhatsappLineDto
            {
                Id = newModel.Id,
                CountryCode = newModel.CountryCode,
                Number = newModel.Number,
                IsPrimary = newModel.IsPrimary
            }, HttpStatusCode.Created);
        }

        public ResponseModel<AppSettingWhatsappLineDto> DeleteAppSettingWhatsappLine(int id)
        {
           var result = _entityRepository.GetForEdit(x => x.Id == id);
            if (result == null)
            {
                return ResponseModel<AppSettingWhatsappLineDto>.Fail("Kayıt bulunamadı.", HttpStatusCode.NotFound);
            }
            result.IsDeleted = true;
            _entityRepository.Update(result);
            _unitOfWork.SaveHotelChanges();
            var whatsappLineDto = new AppSettingWhatsappLineDto
            {
                Id = result.Id,
                CountryCode = result.CountryCode,
                Number = result.Number,
                IsPrimary = result.IsPrimary
            };
            return ResponseModel<AppSettingWhatsappLineDto>.Success(whatsappLineDto, HttpStatusCode.OK);
        }

        public ResponseModel<AppSettingWhatsappLineDto> EditAppSettingWhatsappLine(AppSettingWhatsappLineEditDto model)
        {
            var result = _entityRepository.GetForEdit(x => x.Id == model.Id);
            if (result == null)
            {
                return ResponseModel<AppSettingWhatsappLineDto>.Fail("Kayıt bulunamadı.", HttpStatusCode.NotFound);
            }
            var controlModel = _entityRepository.GetForEdit(w => w.CountryCode == model.CountryCode && w.Number == model.Number && w.Id != model.Id);
            if (controlModel != null)
            {
                return ResponseModel<AppSettingWhatsappLineDto>.Fail("Bu kayıt zaten mevcut.", HttpStatusCode.Conflict);
            }
            result.CountryCode = model.CountryCode;
            result.Number = model.Number;
            result.IsPrimary = model.IsPrimary;
            _entityRepository.Update(result);
            _unitOfWork.SaveHotelChanges();
            var whatsappLineDto = new AppSettingWhatsappLineDto
            {
                Id = result.Id,
                CountryCode = result.CountryCode,
                Number = result.Number,
                IsPrimary = result.IsPrimary
            };
            return ResponseModel<AppSettingWhatsappLineDto>.Success(whatsappLineDto, HttpStatusCode.OK);
        }

        public ResponseModel<IEnumerable<AppSettingWhatsappLineDto>> GetAllAppSettingWhatsappLine(bool isDeleted)
        {
            var result = _entityRepository.WhereForRead(x => x.IsDeleted == isDeleted);
            if (result == null)
            {
                return ResponseModel<IEnumerable<AppSettingWhatsappLineDto>>.Fail("Kayıt bulunamadı.", HttpStatusCode.NotFound);
            }
            var whatsappLineDtos = result.Select(x => new AppSettingWhatsappLineDto
            {
                Id = x.Id,
                CountryCode = x.CountryCode,
                Number = x.Number,
                IsPrimary = x.IsPrimary
            }).ToList();
            return ResponseModel<IEnumerable<AppSettingWhatsappLineDto>>.Success(whatsappLineDtos, HttpStatusCode.OK);
        }

        public ResponseModel<AppSettingWhatsappLineDto> GetAppSettingWhatsappLine(int id, bool isDeleted)
        {
            var result =_entityRepository.GetForRead(x => x.Id == id && x.IsDeleted == isDeleted);
            if (result == null)
            {
                return ResponseModel<AppSettingWhatsappLineDto>.Fail("Kayıt bulunamadı.", HttpStatusCode.NotFound);
            }
            var whatsappLineDto = new AppSettingWhatsappLineDto
            {
                Id = result.Id,
                CountryCode = result.CountryCode,
                Number = result.Number,
                IsPrimary = result.IsPrimary
            };
            return ResponseModel<AppSettingWhatsappLineDto>.Success(whatsappLineDto, HttpStatusCode.OK);
        }
    }
}
