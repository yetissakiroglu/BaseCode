using Microsoft.AspNetCore.Mvc.Rendering;

namespace Economy.Infrastructure.Currencies
{
    public interface ICurrencyService
    {
        ICollection<SelectListItem> GetCurrencies();
    }
}
