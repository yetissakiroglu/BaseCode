using Economy.Application.TenantUI.Dtos;
using Economy.Application.TenantUI.Dtos.AppPageDtos;
using Economy.Core.Enums;
using Economy.Core.Tools;
using Economy.Core.Tools.Result;

namespace Economy.Application.TenantUI.Interfaces
{
    public interface IPanelAppPageService
    {
        Task<ServiceResult<NoContent>> FillLanguagesAsync(PageEditDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> EnsureLanguageTabsAsync(PageEditDto vm, CancellationToken ct);
        Task<ServiceResult<List<PageMiniListDto>>> GetPageMiniListAsync(bool onlyActive, CancellationToken ct);
        Task<ServiceResult<PageEditDto>> GetPageAsync(int id, CancellationToken ct);
        Task<ServiceResult<List<PageListDto>>> GetPageListAsync(CancellationToken ct);
        Task<ServiceResult<List<PageListDto>>> GetPageListAsync(ContentItemType type ,CancellationToken ct);
        Task<ServiceResult<List<PageListDto>>> GetPageListPagingAsync(ContentItemType type, int pageNumber, int pageSize, CancellationToken ct);



        Task<ServiceResult<List<PageParentOptionDto>>> GetParentOptionsAsync(CancellationToken ct, int? excludeId = null);
        Task<ServiceResult<NoContent>> Create(PageEditDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> Edit(int id, PageEditDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> CreateEdit(PageEditDto vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> Delete(int id, CancellationToken ct);



        Task<ServiceResult<NoContent>> RoomAttributeFillLanguagesAsync(RoomAttributeEditVm vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> RoomAttributeEnsureLanguageTabsAsync(RoomAttributeEditVm vm, CancellationToken ct);
        Task<ServiceResult<RoomAttributeEditVm>> GetRoomAttributeAsync(int attributeId, CancellationToken ct);
        Task<ServiceResult<NoContent>> CreateEditRoomAttribute(RoomAttributeEditVm vm, CancellationToken ct);

        Task<ServiceResult<List<RoomAttributeListVm>>> GetRoomAttributeListAsync(CancellationToken ct);
        Task<ServiceResult<List<RoomAttributeOptionListVm>>> GetRoomAttributeOptionListAsync(int attributeId, CancellationToken ct);
        Task<ServiceResult<RoomAttributeValueEditVm>> GetPageEditAttributesAsync(int pageId, CancellationToken ct);
        Task<ServiceResult<RoomAttributeOptionEditVm>> GetRoomAttributeOptionAsync(int attributeOptionId, CancellationToken ct);
        Task<ServiceResult<NoContent>> RoomAttributeOptionFillLanguagesAsync(RoomAttributeOptionEditVm vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> RoomAttributeOptionEnsureLanguageTabsAsync(RoomAttributeOptionEditVm vm, CancellationToken ct);
        Task<ServiceResult<NoContent>> CreateEditRoomAttributeOptionAsync(RoomAttributeOptionEditVm vm, CancellationToken ct);


    }
}
