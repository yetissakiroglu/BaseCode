using Economy.Base.Application.Dtos.BaseModels;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Core.Tools.Models;
using Economy.Panel.Application.Dtos.AppSettingDtos;
using Economy.Panel.Application.Interfaces;
using Economy.Panel.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Panel.Persistence.Services
{
    public class PanelAppSettingService : IPanelAppSettingService
    {
        private readonly PanelAppSettingRepository _panelAppSettingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PanelAppSettingService(PanelAppSettingRepository panelAppSettingRepository, IUnitOfWork unitOfWork)
        {
            _panelAppSettingRepository = panelAppSettingRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<ResponseModel<AppSettingDto>> CreateAppSetting(AppSettingCreateDto appSettingCreateDto)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppSettingDto> DeleteAppSetting(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel<AppSettingDto>> EditAppSetting(AppSettingEditDto appSettingEditDto)
        {
            throw new NotImplementedException();
        }

        public ResponseModel<AppSettingDto> GetAppSetting(int id, bool isDeleted)
        {

            var result = _panelAppSettingRepository.GetForRead(w => w.Id == id && w.IsDeleted == isDeleted);
            return new ResponseModel<AppSettingDto>()
            {
                IsSuccess = true,
                Data = new AppSettingDto
                {
                    Id=result.Id
                },
                Message = new ResultMessage("Ayarlar Başarıyla Getirildi"),
                Notification = Core.Enums.NotificationType.Success,
                Status = HttpStatusCode.OK
            };

        }
    }
}
