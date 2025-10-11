using Economy.Application.AdminUI.Dtos.DashboardSummaryDtos;
using Economy.Application.AdminUI.Interfaces;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.AdminEntity.EntityApp;
using Economy.Domain.Entites.AdminEntity.EntityAppUsers;

namespace Economy.Persistence.Admin.Services
{


    public class PanelDashboardService : IPanelDashboardService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEntityRepository<AppUser, int> _userRepo;
        private readonly IEntityRepository<AppDatabaseBackupLog, int>? _backupRepo; // opsiyonel

        public PanelDashboardService(IUnitOfWork uow)
        {
            _uow = uow;
            _userRepo = uow.DefaultEntityRepository<AppUser>();
            try { _backupRepo = uow.DefaultEntityRepository<AppDatabaseBackupLog>(); } catch { }
        }

        public async Task<ServiceResult<DashboardSummaryDto>> GetSummaryAsync()
        {
            var nowUtc = DateTime.UtcNow;
            var from24 = nowUtc.AddHours(-23).AddMinutes(-nowUtc.Minute).AddSeconds(-nowUtc.Second).AddMilliseconds(-nowUtc.Millisecond);
            var from7d = nowUtc.Date.AddDays(-6);

            // Cards
            var totalUsers = _userRepo.WhereForRead(_ => true).Count();
            var activeUsers = _userRepo.WhereForRead(x => !x.IsDeleted).Count();
            var locked = _userRepo.WhereForRead(x => x.LockoutEnd.HasValue && x.LockoutEnd > nowUtc).Count();
            var twofa = _userRepo.WhereForRead(x => x.TwoFactorEnabled).Count();

           
            DateTime? lastBackup = null;
            if (_backupRepo != null)
            {
                lastBackup = _backupRepo.WhereForRead(x => !x.IsDeleted && x.Succeeded)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .Select(x => (DateTime?)x.CreatedAt)
                                        .FirstOrDefault();
            }
           
            var dto = new DashboardSummaryDto
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                LockedOutUsers = locked,
                TwoFactorEnabledUsers = twofa,
                LastBackupUtc = lastBackup
            };

            return ServiceResult<DashboardSummaryDto>.Success(dto);
        }
    }

}
