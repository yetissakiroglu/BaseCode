using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Domain.Entites.EntityAppSettings;
using Economy.Panel.Application.Dtos.AppSettingWhatsappLineDtos;
using Economy.Panel.Application.Interfaces;

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
            throw new NotImplementedException();
        }

        public ResponseModel<AppSettingWhatsappLineDto> DeleteAppSettingWhatsappLine(int id)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppSettingWhatsappLineDto> EditAppSettingWhatsappLine(AppSettingWhatsappLineEditDto model)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<IEnumerable<AppSettingWhatsappLineDto>> GetAllAppSettingWhatsappLine(bool isDeleted)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppSettingWhatsappLineDto> GetAppSettingWhatsappLine(int id, bool isDeleted)
        {
            throw new NotImplementedException();
        }
    }
}
