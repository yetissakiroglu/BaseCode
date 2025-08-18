using Economy.Application.Dtos;
using Economy.Application.Dtos.AppErrorLogDtos;
using Economy.Application.Interfaces;
using Economy.Core.Interfaces;
using Economy.Core.Tools.Result;
using Economy.Domain.Entites.AppEntities;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Economy.Persistence.Services
{
    public class PanelErrorLogService : IPanelErrorLogService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEntityRepository<AppErrorLog, int> _repo;

        public PanelErrorLogService(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = uow.DefaultEntityRepository<AppErrorLog>();
        }

        public Task<ServiceResult<ErrorLogDto>> GetAsync(long id)
        {
            var e = _repo.GetForRead(x => x.Id == id && !x.IsDeleted);
            if (e == null)
                return Task.FromResult(ServiceResult<ErrorLogDto>.Failure("Kayıt bulunamadı.", statusCode: (int)HttpStatusCode.NotFound));

            return Task.FromResult(ServiceResult<ErrorLogDto>.Success(Map(e)));
        }

        public async Task<ServiceResult<PagedResult<ErrorLogDto>>> ListAsync(ErrorLogQueryDto q)
        {
            IEnumerable<AppErrorLog> qry = _repo.WhereForRead(x => !x.IsDeleted);

            if (q.DateFrom is DateTime fromUtc)
                qry = qry.Where(x => x.CreatedAt >= fromUtc);

            if (q.DateTo is DateTime toUtc)
            {
                var endExclusive = toUtc.Date.AddDays(1);
                qry = qry.Where(x => x.CreatedAt < endExclusive);
            }

            if (q.UserId.HasValue)
                qry = qry.Where(x => x.UserId == q.UserId);

            if (!string.IsNullOrWhiteSpace(q.UserName))
            {
                var un = q.UserName.Trim();
                qry = qry.Where(x => x.UserName != null && EF.Functions.Like(x.UserName, $"%{un}%"));
            }

            if (q.StatusCode.HasValue)
                qry = qry.Where(x => x.StatusCode == q.StatusCode);

            if (!string.IsNullOrWhiteSpace(q.CorrelationId))
            {
                var cid = q.CorrelationId.Trim();
                qry = qry.Where(x => x.CorrelationId == cid);
            }

            if (!string.IsNullOrWhiteSpace(q.ExceptionType))
            {
                var et = q.ExceptionType.Trim();
                qry = qry.Where(x => x.ExceptionType == et);
            }

            if (!string.IsNullOrWhiteSpace(q.Keyword))
            {
                var k = q.Keyword.Trim();
                qry = qry.Where(x =>
                    (x.Message != null && EF.Functions.Like(x.Message, $"%{k}%")) ||
                    (x.StackTrace != null && EF.Functions.Like(x.StackTrace, $"%{k}%")) ||
                    (x.Source != null && EF.Functions.Like(x.Source, $"%{k}%")) ||
                    (x.RequestPath != null && EF.Functions.Like(x.RequestPath, $"%{k}%")));
            }

            // Sıralama
            qry = (q.Sort?.ToLowerInvariant()) switch
            {
                "created" => qry.OrderBy(x => x.CreatedAt),
                "-status" => qry.OrderByDescending(x => x.StatusCode),
                "status" => qry.OrderBy(x => x.StatusCode),
                _ => qry.OrderByDescending(x => x.CreatedAt) // -created default
            };

            var total = qry.Count(); // veya await CountAsync()
            var page = Math.Max(1, q.Page);
            var size = Math.Clamp(q.PageSize, 5, 200);

            var items = qry.Skip((page - 1) * size).Take(size)
                .Select(x => new ErrorLogDto
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    UserId = x.UserId,
                    UserName = x.UserName,
                    HttpMethod = x.HttpMethod,
                    RequestPath = x.RequestPath,
                    QueryString = x.QueryString,
                    IpAddress = x.IpAddress,
                    UserAgent = x.UserAgent,
                    CorrelationId = x.CorrelationId,
                    StatusCode = x.StatusCode,
                    ExceptionType = x.ExceptionType,
                    Message = x.Message,
                    StackTrace = x.StackTrace,
                    Source = x.Source,
                    TargetSite = x.TargetSite,
                    HeadersJson = x.HeadersJson,
                    RequestBodyTruncated = x.RequestBodyTruncated,
                    CustomDataJson = x.CustomDataJson
                })
                .ToList();

            var result = new PagedResult<ErrorLogDto>
            {
                Page = page,
                PageSize = size,
                TotalItems = total,
                Items = items
            };

            return ServiceResult<PagedResult<ErrorLogDto>>.Success(result);
        }

        public async Task<ServiceResult<int>> ClearAsync(DateTime? olderThanUtc = null)
        {
            var cut = olderThanUtc ?? DateTime.UtcNow.AddDays(-30);
            var toDelete = _repo.WhereForEdit(x => !x.IsDeleted && x.CreatedAt < cut).ToList();
            foreach (var e in toDelete) _repo.Delete(e);
            await _uow.SaveDefaultChangesAsync();
            return ServiceResult<int>.Success(toDelete.Count, "Eski hata logları temizlendi.");
        }

        public async Task<ServiceResult<ErrorLogDto>> DeleteAsync(long id)
        {
            var e = _repo.GetForEdit(x => x.Id == id && !x.IsDeleted);
            if (e == null)
                return ServiceResult<ErrorLogDto>.Failure("Kayıt bulunamadı.", statusCode: (int)HttpStatusCode.NotFound);

            _repo.Delete(e);
            await _uow.SaveDefaultChangesAsync();
            return ServiceResult<ErrorLogDto>.Success(Map(e), "Kayıt silindi.");
        }

        private static ErrorLogDto Map(AppErrorLog x) => new()
        {
            Id = x.Id,
            CreatedAt = x.CreatedAt,
            UserId = x.UserId,
            UserName = x.UserName,
            HttpMethod = x.HttpMethod,
            RequestPath = x.RequestPath,
            QueryString = x.QueryString,
            IpAddress = x.IpAddress,
            UserAgent = x.UserAgent,
            CorrelationId = x.CorrelationId,
            StatusCode = x.StatusCode,
            ExceptionType = x.ExceptionType,
            Message = x.Message,
            StackTrace = x.StackTrace,
            Source = x.Source,
            TargetSite = x.TargetSite,
            HeadersJson = x.HeadersJson,
            RequestBodyTruncated = x.RequestBodyTruncated,
            CustomDataJson = x.CustomDataJson
        };
    }
}
