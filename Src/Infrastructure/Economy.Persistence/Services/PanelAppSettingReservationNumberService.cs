using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Panel.Application.Dtos.AppSettingReservationNumberDtos;
using Economy.Panel.Application.Interfaces;
using System.Net;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppSettingReservationNumberService : IPanelAppSettingReservationNumberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppSettingReservationNumber, int> _entityRepository;

        public PanelAppSettingReservationNumberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _entityRepository = unitOfWork.HotelEntityRepository<AppSettingReservationNumber>();
        }
        public ResponseModel<AppSettingReservationNumberDto> CreateReservationNumber(AppSettingReservationNumberCreateDto model)
        {
            var controlModel = _entityRepository.GetForEdit(w => w.CountryCode == model.CountryCode && w.Number == model.Number);
            if (controlModel != null)
            {
                return ResponseModel<AppSettingReservationNumberDto>.Fail("Bu kayıt zaten mevcut.", HttpStatusCode.Conflict);
            }
            if (string.IsNullOrEmpty(model.CountryCode))
            {
                return ResponseModel<AppSettingReservationNumberDto>.Fail("Ülke kodu boş olamaz.", HttpStatusCode.BadRequest);
            }
            if (string.IsNullOrEmpty(model.Number))
            {
                return ResponseModel<AppSettingReservationNumberDto>.Fail("Numara boş olamaz.", HttpStatusCode.BadRequest);
            }
            if (model.Number.Length < 10)
            {
                return ResponseModel<AppSettingReservationNumberDto>.Fail("Numara en az 10 karakter olmalıdır.", HttpStatusCode.BadRequest);
            }
            if (model.IsPrimary)
            {
                var primaryNumber = _entityRepository.GetForEdit(x => x.IsPrimary == true && x.CountryCode == model.CountryCode);
                if (primaryNumber != null)
                {
                    primaryNumber.IsPrimary = false;
                    _entityRepository.Update(primaryNumber);
                }
            }

            var newModel = new AppSettingReservationNumber
            {
                CountryCode = model.CountryCode,
                Number = model.Number,
                IsPrimary = model.IsPrimary
            };
            _entityRepository.Add(newModel);
            _unitOfWork.SaveHotelChanges();
            return ResponseModel<AppSettingReservationNumberDto>.Success(new AppSettingReservationNumberDto
            {
                Id = newModel.Id,
                CountryCode = newModel.CountryCode,
                Number = newModel.Number,
                IsPrimary = newModel.IsPrimary
            }, HttpStatusCode.Created);

        }

        public ResponseModel<AppSettingReservationNumberDto> DeleteReservationNumber(int id)
        {
            var result = _entityRepository.GetForEdit(x => x.Id == id);
            if (result == null)
            {
                return ResponseModel<AppSettingReservationNumberDto>.Fail("Kayıt bulunamadı.", HttpStatusCode.NotFound);
            }
            result.IsDeleted = true;
            _entityRepository.Update(result);
            _unitOfWork.SaveHotelChanges();
            var reservationNumberDto = new AppSettingReservationNumberDto
            {
                Id = result.Id,
                CountryCode = result.CountryCode,
                Number = result.Number,
                IsPrimary = result.IsPrimary
            };
            return ResponseModel<AppSettingReservationNumberDto>.Success(reservationNumberDto, HttpStatusCode.OK);
        }

        public ResponseModel<AppSettingReservationNumberDto> EditReservationNumber(AppSettingReservationNumberEditDto model)
        {
            var result = _entityRepository.GetForEdit(x => x.Id == model.Id);
            if (result == null)
            {
                return ResponseModel<AppSettingReservationNumberDto>.Fail("Kayıt bulunamadı.", HttpStatusCode.NotFound);
            }
  
            result.CountryCode = model.CountryCode;
            result.Number = model.Number;
            result.IsPrimary = model.IsPrimary;
            _entityRepository.Update(result);
            _unitOfWork.SaveHotelChanges();
            var reservationNumberDto = new AppSettingReservationNumberDto
            {
                Id = result.Id,
                CountryCode = result.CountryCode,
                Number = result.Number,
                IsPrimary = result.IsPrimary
            };
            return ResponseModel<AppSettingReservationNumberDto>.Success(reservationNumberDto, HttpStatusCode.OK);
        }

        public ResponseModel<IEnumerable<AppSettingReservationNumberDto>> GetAllReservationNumber(bool isDeleted)
        {
            var result = _entityRepository.WhereForRead(x => x.IsDeleted == isDeleted)
                            .Select(x => new AppSettingReservationNumberDto
                            {
                                Id = x.Id,
                                CountryCode = x.CountryCode,
                                Number = x.Number,
                                IsPrimary = x.IsPrimary
                            }).ToList();


            if (!result.Any())
                return ResponseModel<IEnumerable<AppSettingReservationNumberDto>>.Success(HttpStatusCode.OK);


            return ResponseModel<IEnumerable<AppSettingReservationNumberDto>>.Success(result, System.Net.HttpStatusCode.OK);

        }

        public ResponseModel<AppSettingReservationNumberDto> GetReservationNumber(int id, bool isDeleted)
        {
            var result = _entityRepository.GetForReadFunc(x => x.Id == id && x.IsDeleted == isDeleted);
            if (result == null)
                return ResponseModel<AppSettingReservationNumberDto>.Fail("Kayıt bulunamadı.", HttpStatusCode.NotFound);
            var reservationNumberDto = new AppSettingReservationNumberDto()
            {
                Id = result.Id,
                CountryCode = result.CountryCode,
                Number = result.Number,
                IsPrimary = result.IsPrimary
            };
            return ResponseModel<AppSettingReservationNumberDto>.Success(reservationNumberDto, HttpStatusCode.OK);
        }
    }
}
