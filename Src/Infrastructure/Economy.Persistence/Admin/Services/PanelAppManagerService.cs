using Economy.Application.AdminUI.Interfaces;
using Economy.Core.Interfaces;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.AdminEntity.EntityApp;
using Economy.Persistence.Repositories.AppBase.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Economy.Persistence.Admin.Services
{
    public class PanelAppManagerService: IPanelAppManagerService
    {
        private readonly IEntityRepository<AppManager, int> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public PanelAppManagerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _repository = unitOfWork.DefaultEntityRepository<AppManager>();
        }

        public async Task<List<int>> GetManagerIdsByAppIdAsync(int appId)
        {
            if (_repository is not EfEntityRepositoryBase<AppManager> efRepo)
                return new List<int>();

            return await efRepo.DataSet
                .AsNoTracking()
                .Where(x => x.AppId == appId && !x.IsDeleted)
                .Select(x => x.UserId)
                .ToListAsync();
        }

        public async Task<ServiceResult<NoContent>> UpdateManagersForAppAsync(int appId, List<int> userIds)
        {
            if (_repository is not EfEntityRepositoryBase<AppManager> efRepo)
            {
                return ServiceResult<NoContent>.Failure(
                    message: "Repository tipi uyumsuz.",
                    statusCode: (int)HttpStatusCode.InternalServerError
                );
            }

            // Eski atamaları kaldır
            var existing = efRepo.DataSet
                .Where(x => x.AppId == appId && !x.IsDeleted)
                .ToList();

            foreach (var old in existing)
            {
                old.IsDeleted = true;
                _repository.Update(old);
            }

            // Yeni atamaları ekle
            foreach (var userId in userIds)
            {
                var newRecord = new AppManager
                {
                    AppId = appId,
                    UserId = userId,
                    IsDeleted = false
                };
                _repository.Add(newRecord);
            }

            _unitOfWork.SaveDefaultChanges();

            return ServiceResult<NoContent>.Success(null,
                message: "Yönetici atamaları güncellendi.",
                statusCode: (int)HttpStatusCode.OK
            );
        }
    }
}
