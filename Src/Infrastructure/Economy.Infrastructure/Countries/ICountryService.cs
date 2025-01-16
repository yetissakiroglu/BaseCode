using Microsoft.AspNetCore.Mvc.Rendering;

namespace Economy.Infrastructure.Countries
{
    public interface ICountryService
    {
        ICollection<SelectListItem> GetCountries();
    }
}
