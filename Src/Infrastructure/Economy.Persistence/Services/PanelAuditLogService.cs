using Economy.Application.Dtos;
using Economy.Application.Dtos.AppAuditLogDtos;
using Economy.Application.Interfaces;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.AppEntities;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Economy.Persistence.Services
{
    public class PanelAuditLogService : IPanelAuditLogService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEntityRepository<AppAuditLog, int> _repo;

        public PanelAuditLogService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = uow.DefaultEntityRepository<AppAuditLog>(); // senin pattern
        }

        public Task<ServiceResult<AuditLogDto>> GetAsync(long id)
        {
            var e = _repo.GetForRead(x => x.Id == id && !x.IsDeleted);
            if (e == null)
                return Task.FromResult(ServiceResult<AuditLogDto>.Failure("Kayıt bulunamadı.",statusCode: (int)HttpStatusCode.NotFound));

            return Task.FromResult(ServiceResult<AuditLogDto>.Success(Map(e)));
        }

        public async Task<ServiceResult<PagedResult<AuditLogDto>>> ListAsync(AuditLogQueryDto q)
        {
            IEnumerable<AppAuditLog> qry = _repo.WhereForRead(x => !x.IsDeleted);

            if (q.DateFrom.HasValue)
                qry = qry.Where(x => x.CreatedDate >= q.DateFrom);

            if (q.DateTo.HasValue)
            {
                var to = q.DateTo.Value.Date.AddDays(1);
                qry = qry.Where(x => x.CreatedDate < to);
            }

            if (q.UserId.HasValue)
                qry = qry.Where(x => x.UserId == q.UserId);

            if (!string.IsNullOrWhiteSpace(q.UserName))
            {
                var un = q.UserName.Trim();
                qry = qry.Where(x => x.UserName != null && EF.Functions.Like(x.UserName, $"%{un}%"));
            }

            if (!string.IsNullOrWhiteSpace(q.Action))
            {
                var a = q.Action.Trim();
                qry = qry.Where(x => x.Action == a);
            }

            if (!string.IsNullOrWhiteSpace(q.EntityName))
            {
                var en = q.EntityName.Trim();
                qry = qry.Where(x => x.EntityName == en);
            }

            if (q.Succeeded.HasValue)
                qry = qry.Where(x => x.Succeeded == q.Succeeded);

            if (q.StatusCode.HasValue)
                qry = qry.Where(x => x.StatusCode == q.StatusCode);

            if (!string.IsNullOrWhiteSpace(q.Keyword))
            {
                var k = q.Keyword.Trim();
                qry = qry.Where(x =>
                    (x.Message != null && EF.Functions.Like(x.Message, $"%{k}%")) ||
                    (x.OldValuesJson != null && EF.Functions.Like(x.OldValuesJson, $"%{k}%")) ||
                    (x.NewValuesJson != null && EF.Functions.Like(x.NewValuesJson, $"%{k}%")) ||
                    (x.AffectedColumnsJson != null && EF.Functions.Like(x.AffectedColumnsJson, $"%{k}%")));
            }

            // Sıralama
            qry = (q.Sort?.ToLowerInvariant()) switch
            {
                "created" => qry.OrderBy(x => x.CreatedDate),
                "-status" => qry.OrderByDescending(x => x.StatusCode),
                "status" => qry.OrderBy(x => x.StatusCode),
                "-duration" => qry.OrderByDescending(x => x.DurationMs),
                "duration" => qry.OrderBy(x => x.DurationMs),
                _ => qry.OrderByDescending(x => x.CreatedDate) // -created
            };

            var total = qry.Count();

            var page = Math.Max(1, q.Page);
            var size = Math.Clamp(q.PageSize, 5, 200);
            var items = qry.Skip((page - 1) * size).Take(size)
                .Select(x => new AuditLogDto
                {
                    Id = x.Id,
                    CreatedDate = x.CreatedDate,
                    UserId = x.UserId,
                    UserName = x.UserName,
                    Action = x.Action,
                    EntityName = x.EntityName,
                    EntityId = x.EntityId,
                    Message = x.Message,
                    HttpMethod = x.HttpMethod,
                    RequestPath = x.RequestPath,
                    IpAddress = x.IpAddress,
                    CorrelationId = x.CorrelationId,
                    Succeeded = x.Succeeded,
                    StatusCode = x.StatusCode,
                    DurationMs = x.DurationMs,
                    OldValuesJson = x.OldValuesJson,
                    NewValuesJson = x.NewValuesJson,
                    AffectedColumnsJson = x.AffectedColumnsJson
                })
                .ToList();

            var result = new PagedResult<AuditLogDto>
            {
                Page = page,
                PageSize = size,
                TotalItems = total,
                Items = items
            };

            return ServiceResult<PagedResult<AuditLogDto>>.Success(result);
        }

        private static AuditLogDto Map(AppAuditLog e) => new()
        {
            Id = e.Id,
            CreatedDate = e.CreatedDate,
            UserId = e.UserId,
            UserName = e.UserName,
            Action = e.Action,
            EntityName = e.EntityName,
            EntityId = e.EntityId,
            Message = e.Message,
            HttpMethod = e.HttpMethod,
            RequestPath = e.RequestPath,
            IpAddress = e.IpAddress,
            CorrelationId = e.CorrelationId,
            Succeeded = e.Succeeded,
            StatusCode = e.StatusCode,
            DurationMs = e.DurationMs,
            OldValuesJson = e.OldValuesJson,
            NewValuesJson = e.NewValuesJson,
            AffectedColumnsJson = e.AffectedColumnsJson
        };
    }
}
