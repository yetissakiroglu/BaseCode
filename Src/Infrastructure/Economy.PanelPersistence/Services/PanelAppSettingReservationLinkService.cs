using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Panel.Application.Dtos.AppSettingReservationLinkDtos;
using Economy.Panel.Application.Interfaces;
using System.Net;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppSettingReservationLinkService : IPanelAppSettingReservationLinkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppSettingReservationLink, int> _entityRepository;

        public PanelAppSettingReservationLinkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _entityRepository = unitOfWork.EntityRepository<AppSettingReservationLink>();
        }

        public ResponseModel<AppSettingReservationLinkDto> CreateReservationLink(AppSettingReservationLinkCreateDto model)
        {
            var reservationLink = _entityRepository.GetForEdit(x => x.Url == model.Url);
            if (reservationLink != null)
            {
                return ResponseModel<AppSettingReservationLinkDto>.Fail("Bu kayıt zaten mevcut.", HttpStatusCode.Conflict);
            }
            if (string.IsNullOrEmpty(model.Url))
            {
                return ResponseModel<AppSettingReservationLinkDto>.Fail("Url boş olamaz.", HttpStatusCode.BadRequest);
            }
         
      
            var newModel = new AppSettingReservationLink
            {
                Url = model.Url,
            };
            _entityRepository.Add(newModel);
            _unitOfWork.SaveHotelChanges();
            return ResponseModel<AppSettingReservationLinkDto>.Success(new AppSettingReservationLinkDto
            {
                Id = newModel.Id,
                Url = newModel.Url,
            }, HttpStatusCode.Created);

        }

        public ResponseModel<AppSettingReservationLinkDto> DeleteReservationLink(int id)
        {
            var result = _entityRepository.GetForEdit(x => x.Id == id);
            if (result == null)
            {
                return ResponseModel<AppSettingReservationLinkDto>.Fail("Bu kayıt bulunamadı.", HttpStatusCode.NotFound);
            }
            result.IsDeleted = true;
            _entityRepository.Update(result);
            _unitOfWork.SaveHotelChanges();
            return ResponseModel<AppSettingReservationLinkDto>.Success(new AppSettingReservationLinkDto
            {
                Id = result.Id,
                Url = result.Url,
            }, HttpStatusCode.OK);

        }

        public ResponseModel<AppSettingReservationLinkDto> EditReservationLink(AppSettingReservationLinkEditDto model)
        {
            var result = _entityRepository.GetForEdit(x => x.Id == model.Id);
            if (result == null)
            {
                return ResponseModel<AppSettingReservationLinkDto>.Fail("Bu kayıt bulunamadı.", HttpStatusCode.NotFound);
            }
            if (string.IsNullOrEmpty(model.Url))
            {
                return ResponseModel<AppSettingReservationLinkDto>.Fail("Url boş olamaz.", HttpStatusCode.BadRequest);
            }
            result.Url = model.Url;
            _entityRepository.Update(result);
            _unitOfWork.SaveHotelChanges();
            return ResponseModel<AppSettingReservationLinkDto>.Success(new AppSettingReservationLinkDto
            {
                Id = result.Id,
                Url = result.Url,
            }, HttpStatusCode.OK);
        }

        public ResponseModel<IEnumerable<AppSettingReservationLinkDto>> GetAllReservationLink(bool isDeleted)
        {
            var result = _entityRepository.WhereForRead(x => x.IsDeleted == isDeleted).ToList();
            var reservationLinkDtos = result.Select(x => new AppSettingReservationLinkDto
            {
                Id = x.Id,
                Url = x.Url,
            }).ToList();
            return ResponseModel<IEnumerable<AppSettingReservationLinkDto>>.Success(reservationLinkDtos, HttpStatusCode.OK);
        }

        public ResponseModel<AppSettingReservationLinkDto> GetReservationLink(int id, bool isDeleted)
        {
           var result = _entityRepository.GetForRead(x => x.Id == id && x.IsDeleted == isDeleted);
            if (result == null)
            {
                return ResponseModel<AppSettingReservationLinkDto>.Fail("Bu kayıt bulunamadı.", HttpStatusCode.NotFound);
            }
            var reservationLinkDto = new AppSettingReservationLinkDto
            {
                Id = result.Id,
                Url = result.Url,
            };
            return ResponseModel<AppSettingReservationLinkDto>.Success(reservationLinkDto, HttpStatusCode.OK);
        }
    }
}
