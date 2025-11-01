using Economy.Application.TenantUI.Dtos.AppPageDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Economy.Application.TenantUI.Interfaces
{
    public interface IPanelPageBlockGroupService
    {
        Task<IReadOnlyList<BlockGroupMiniDto>> ListForPageAsync(int pageId);           // bu sayfaya bağlı bloklar (pivot + group)
        Task<IReadOnlyList<BlockGroupMiniDto>> ListCandidatesAsync(int pageId, string? q = null); // sayfaya bağlı olmayan bloklar (eklemek için)
        Task AttachAsync(int pageId, int blockGroupId);                                 // PageBlock insert
        Task DetachAsync(int pageId, int blockGroupId);                                 // PageBlock delete
        Task SortAsync(int pageId, List<(int blockGroupId, int sortOrder)> pairs);      // PageBlock sort
    }
}
