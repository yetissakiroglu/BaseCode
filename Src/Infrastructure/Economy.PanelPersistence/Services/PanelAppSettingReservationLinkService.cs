using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Panel.Application.Dtos.AppSettingReservationLinkDtos;
using Economy.Panel.Application.Interfaces;

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
            throw new NotImplementedException();
        }

        public ResponseModel<AppSettingReservationLinkDto> DeleteReservationLink(int id)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppSettingReservationLinkDto> EditReservationLink(AppSettingReservationLinkEditDto model)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<IEnumerable<AppSettingReservationLinkDto>> GetAllReservationLink(bool isDeleted)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppSettingReservationLinkDto> GetReservationLink(int id, bool isDeleted)
        {
            throw new NotImplementedException();
        }
    }
}
