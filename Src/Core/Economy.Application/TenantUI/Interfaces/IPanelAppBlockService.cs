using Economy.Application.TenantUI.Dtos.AppBlockDtos;
using Economy.Core.Enums;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;

namespace Economy.Application.TenantUI.Interfaces
{
    public interface IPanelAppBlockService
    {
        Task<ServiceResult<AppBlockListDto>> GetAllBlocksAsync(BlockType? type, int page, int size, string? q);
        Task<ServiceResult<AppBlockDto>> GetBlocksAsync(int blockId, CancellationToken ct);
        Task<ServiceResult<NoContent>> DeleteBlockAsync(int blockId, CancellationToken ct);
        Task<ServiceResult<NoContent>> FillLanguagesAsync(AppBlockDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> FillLanguagesAsync(AppBlockDto vm, BlockType type, CancellationToken ct);
        Task<ServiceResult<NoContent>> EnsureLanguageTabsAsync(AppBlockDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> CreateBlockAsync(AppBlockDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> EditBlockAsync(int blockId, AppBlockDto vm, CancellationToken ct);

    }
}
