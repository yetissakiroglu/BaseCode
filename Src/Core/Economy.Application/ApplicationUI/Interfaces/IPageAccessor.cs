using Economy.UI.Models.PageDtos;
using Microsoft.AspNetCore.Http;

namespace Economy.Application.ApplicationUI.Interfaces
{
    public interface IPageAccessor
    {
        /// <summary>
        /// Tek endpoint servis metodu:
        /// slug = rooms / campaigns => liste
        /// slug = {page-slug}       => detay
        /// </summary>
        Task<PageUnifiedVm?> GetAsync(string lang, string slug, CancellationToken ct = default);
        Task<PageUnifiedVm?> GetAsync(string lang, bool ishomepage, CancellationToken ct = default);

    }
}
