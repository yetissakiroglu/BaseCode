using Economy.Application.Dtos.DashboardSummaryDtos;
using Economy.Application.Interfaces;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.AppEntities;
using Economy.Domain.Entites.Identities;
namespace Economy.Persistence.Services
{


    public class PanelDashboardService : IPanelDashboardService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEntityRepository<AppUser, int> _userRepo;
        private readonly IEntityRepository<AppAuditLog, int> _auditRepo;
        private readonly IEntityRepository<AppErrorLog, int> _errorRepo;
        private readonly IEntityRepository<AppDatabaseBackupLog, int>? _backupRepo; // opsiyonel

        public PanelDashboardService(IUnitOfWork uow)
        {
            _uow = uow;
            _userRepo = uow.DefaultEntityRepository<AppUser>();
            _auditRepo = uow.DefaultEntityRepository<AppAuditLog>();
            _errorRepo = uow.DefaultEntityRepository<AppErrorLog>();
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

            var errors24 = _errorRepo.WhereForRead(x => !x.IsDeleted && x.CreatedAt >= from24).Count();
            var failedLogins24 = _auditRepo.WhereForRead(x => !x.IsDeleted && x.Action == "Login" && !x.Succeeded && x.CreatedDate >= from24).Count();

            DateTime? lastBackup = null;
            if (_backupRepo != null)
            {
                lastBackup = _backupRepo.WhereForRead(x => !x.IsDeleted && x.Succeeded)
                                        .OrderByDescending(x => x.CreatedAt)
                                        .Select(x => (DateTime?)x.CreatedAt)
                                        .FirstOrDefault();
            }

            // Charts: Errors per hour (24h)
            var errorsGrouped = _errorRepo.WhereForRead(x => !x.IsDeleted && x.CreatedAt >= from24)
                .GroupBy(x => new { x.CreatedAt.Year, x.CreatedAt.Month, x.CreatedAt.Day, x.CreatedAt.Hour })
                .Select(g => new { g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, Count = g.Count() })
                .ToList();

            var failedLoginsGrouped = _auditRepo.WhereForRead(x => !x.IsDeleted && x.Action == "Login" && !x.Succeeded && x.CreatedDate >= from24)
                //.GroupBy(x => new { x.CreatedDate.Value.Year, x.CreatedDate.Value.Month, x.CreatedDate.Value.Day, x.CreatedDate.Value.Hour })
                //.Select(g => new { g.Key.Year, g.Key.Month, g.Key.Day, g.Key.Hour, Count = g.Count() })
                .ToList();

            // 24 saatlik eksiksiz seriler
            var errorsSeries = new List<TimePoint>();
            var failedSeries = new List<TimePoint>();
            for (int i = 0; i < 24; i++)
            {
                var t = from24.AddHours(i);
                var eCount = errorsGrouped
                    .Where(a => a.Year == t.Year && a.Month == t.Month && a.Day == t.Day && a.Hour == t.Hour)
                    .Select(a => a.Count).FirstOrDefault();
                var fCount = failedLoginsGrouped
                    //.Where(a => a.Year == t.Year && a.Month == t.Month && a.Day == t.Day && a.Hour == t.Hour)
                    .Select(a => a.Id).FirstOrDefault();

                var label = t.ToLocalTime().ToString("HH:mm");
                errorsSeries.Add(new TimePoint { Label = label, Value = eCount });
                failedSeries.Add(new TimePoint { Label = label, Value = fCount });
            }

            // New users per day (7d)
            var newUsersGrouped = _userRepo.WhereForRead(x => !x.IsDeleted /*&& x.CreatedDate >= from7d*/) // AppUser'da CreatedDate varsa; yoksa kendi alanını kullan
                //.GroupBy(x => new { x.CreatedDate.Value.Year, x.CreatedDate.Value.Month, x.CreatedDate.Value.Day })
                //.Select(g => new { g.Key.Year, g.Key.Month, g.Key.Day, Count = g.Count() })
                .ToList();

            var newUsers7d = new List<TimePoint>();
            for (int i = 0; i < 7; i++)
            {
                var d = from7d.AddDays(i);
                var c = newUsersGrouped
                    //.Where(a => a.Year == d.Year && a.Month == d.Month && a.Day == d.Day)
                    .Select(a => a.Id).FirstOrDefault();
                newUsers7d.Add(new TimePoint { Label = d.ToLocalTime().ToString("dd MMM"), Value = c });
            }

            // Recent Errors (son 10)
            var recentErrors = _errorRepo.WhereForRead(x => !x.IsDeleted)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new RecentErrorDto
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    Message = x.Message,
                    RequestPath = x.RequestPath,
                    StatusCode = x.StatusCode,
                    ExceptionType = x.ExceptionType
                })
                .Take(10)
                .ToList();

            var dto = new DashboardSummaryDto
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                LockedOutUsers = locked,
                TwoFactorEnabledUsers = twofa,
                ErrorsLast24h = errors24,
                FailedLoginsLast24h = failedLogins24,
                LastBackupUtc = lastBackup,
                ErrorsPerHour24h = errorsSeries,
                FailedLoginsPerHour24h = failedSeries,
                NewUsersPerDay7d = newUsers7d,
                RecentErrors = recentErrors
            };

            return ServiceResult<DashboardSummaryDto>.Success(dto);
        }
    }

}
