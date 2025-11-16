using Economy.UI.Dtos;
using System.Collections.Generic;

namespace HotelMultiTenant.Multitenancy
{
    public interface ISeoHelper
    {
        Task<SeoDto> BuildAsync(
          IReadOnlyList<HreflangVm> hreflang,
           SeoSeed seed,
           string currentLang,
           string? xDefaultUrl = null
        );
    }
}
