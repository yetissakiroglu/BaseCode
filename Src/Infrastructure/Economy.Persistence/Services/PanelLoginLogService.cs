using Economy.Application.Dtos.LoginLogPageQueryDto;
using Economy.Application.Interfaces;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.AppEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<ServiceResult<byte[]>> ExportCsvAsync(LoginLogPageQuery q, int maxRows = 5000)
        {
            // Aynı filtreleri uygula, ama sayfalama yok (maxRows sınırı var)
            var res = GetPageBaseQuery(q)
                .OrderByDescending(x => x.CreatedDate)
                .ThenByDescending(x => x.Id)
                .Take(maxRows)
                .Select(x => new
                {
                    x.Id,
                    x.CreatedDate,
                    x.UserName,
                    x.Succeeded,
                    x.StatusCode,
                    x.Message,
                    x.IpAddress,
                    x.RequestPath,
                    x.CorrelationId,
                    x.UserAgent
                })
                .ToList();

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Id,CreatedUtc,UserName,Succeeded,StatusCode,Message,IpAddress,RequestPath,CorrelationId,UserAgent");
            foreach (var r in res)
            {
                string esc(string? s) => string.IsNullOrEmpty(s) ? "" : "\"" + s.Replace("\"", "\"\"") + "\"";
                sb.Append(r.Id).Append(',')
                  .Append(r.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss")).Append(',')
                  .Append(esc(r.UserName)).Append(',')
                  .Append(r.Succeeded ? "1" : "0").Append(',')
                  .Append(r.StatusCode?.ToString() ?? "").Append(',')
                  .Append(esc(r.Message)).Append(',')
                  .Append(esc(r.IpAddress)).Append(',')
                  .Append(esc(r.RequestPath)).Append(',')
                  .Append(esc(r.CorrelationId)).Append(',')
                  .Append(esc(r.UserAgent)).AppendLine();
            }

            var bytes = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            return Task.FromResult(ServiceResult<byte[]>.Success(bytes, "CSV hazır."));
        }

        // Ortak filtre
        private IQueryable<AppAuditLog> GetPageBaseQuery(LoginLogPageQuery q)
        {
            if (q is null) q = new LoginLogPageQuery();
            IEnumerable<AppAuditLog> qry = _repo.WhereForRead(x => !x.IsDeleted && x.Action == "Login");

            if (q.DateFrom.HasValue)
            {
                var fromUtc = DateTime.SpecifyKind(q.DateFrom.Value.Date, DateTimeKind.Local).ToUniversalTime();
                qry = qry.Where(x => x.CreatedDate >= fromUtc);
            }
            if (q.DateTo.HasValue)
            {
                var toUtcExclusive = DateTime.SpecifyKind(q.DateTo.Value.Date.AddDays(1), DateTimeKind.Local).ToUniversalTime();
                qry = qry.Where(x => x.CreatedDate < toUtcExclusive);
            }
            if (!string.IsNullOrWhiteSpace(q.UserName))
            {
                var u = q.UserName.Trim();
                qry = qry.Where(x => x.UserName != null && x.UserName.Contains(u));
            }
            if (q.Succeeded.HasValue)
                qry = qry.Where(x => x.Succeeded == q.Succeeded.Value);
            if (q.StatusCode.HasValue)
                qry = qry.Where(x => x.StatusCode == q.StatusCode.Value);

            return null;
        }
    }
}
