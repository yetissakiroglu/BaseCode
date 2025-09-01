using Economy.Application.Dtos.LoginLogPageQueryDto;
using Economy.Application.Interfaces;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.AppEntities;

namespace Economy.Persistence.Services
{
    public class PanelLoginLogService : IPanelLoginLogService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEntityRepository<AppAuditLog, int> _repo;

        public PanelLoginLogService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = uow.DefaultEntityRepository<AppAuditLog>();
        }

        public Task<ServiceResult<LoginLogPageViewModel>> GetPageAsync(LoginLogPageQuery q)
        {
            if (q is null) q = new LoginLogPageQuery();

            IEnumerable<AppAuditLog> qry = _repo.WhereForRead(x => !x.IsDeleted && x.Action == "Login");

            if (q.DateFrom.HasValue)
            {
                // UI yerel tarihi UTC'ye çeviriyorsan burada direkt kullan; değilse From için gün başlangıcı varsayabilirsin.
                var fromUtc = DateTime.SpecifyKind(q.DateFrom.Value.Date, DateTimeKind.Local).ToUniversalTime();
                qry = qry.Where(x => x.CreatedDate >= fromUtc);
            }
            if (q.DateTo.HasValue)
            {
                // dahil son gün: +1 gün - 1 tick
                var toUtcExclusive = DateTime.SpecifyKind(q.DateTo.Value.Date.AddDays(1), DateTimeKind.Local).ToUniversalTime();
                qry = qry.Where(x => x.CreatedDate < toUtcExclusive);
            }
            if (!string.IsNullOrWhiteSpace(q.UserName))
            {
                var u = q.UserName.Trim();
                qry = qry.Where(x => x.UserName != null && x.UserName.Contains(u));
            }
            if (q.Succeeded.HasValue)
            {
                var s = q.Succeeded.Value;
                qry = qry.Where(x => x.Succeeded == s);
            }
            if (q.StatusCode.HasValue)
            {
                var sc = q.StatusCode.Value;
                qry = qry.Where(x => x.StatusCode == sc);
            }

            // Toplam
            var total = qry.Count();

            // Sayfalama
            var page = q.Page <= 0 ? 1 : q.Page;
            var size = q.PageSize <= 0 ? 25 : q.PageSize;
            var skip = (page - 1) * size;

            var items = qry
                .OrderByDescending(x => x.CreatedDate)
                .ThenByDescending(x => x.Id)
                .Skip(skip)
                .Take(size)
                .Select(x => new LoginLogRow
                {
                    Id = x.Id,
                    CreatedUtc = x.CreatedDate,        // DB'de UTC tuttuğunu varsayıyoruz
                    UserName = x.UserName,
                    Succeeded = x.Succeeded,
                    StatusCode = x.StatusCode,
                    Message = x.Message,
                    IpAddress = x.IpAddress,
                    RequestPath = x.RequestPath,
                    UserAgent = x.UserAgent,
                    CorrelationId = x.CorrelationId
                })
                .ToList();

            var vm = new LoginLogPageViewModel
            {
                Q = q,
                Items = items,
                TotalCount = total,
                Page = page,
                PageSize = size
            };

            return Task.FromResult(ServiceResult<LoginLogPageViewModel>.Success(vm));
        }

    }
}
