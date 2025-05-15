using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Panel.Application.Dtos.AppSettingReservationNumberDtos;
using Economy.Panel.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppSettingReservationNumberService : IPanelAppSettingReservationNumberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEntityRepository<AppSettingReservationNumber, int> _entityRepository;

        public PanelAppSettingReservationNumberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _entityRepository = unitOfWork.EntityRepository<AppSettingReservationNumber>();
        }
        public ResponseModel<AppSettingReservationNumberDto> CreateReservationNumber(AppSettingReservationNumberCreateDto model)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppSettingReservationNumberDto> DeleteReservationNumber(int id)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppSettingReservationNumberDto> EditReservationNumber(AppSettingReservationNumberEditDto model)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<IEnumerable<AppSettingReservationNumberDto>> GetAllReservationLink(bool isDeleted)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppSettingReservationNumberDto> GetReservationNumber(int id, bool isDeleted)
        {
            throw new NotImplementedException();
        }
    }
}
