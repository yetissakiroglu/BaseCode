using Microsoft.AspNetCore.Mvc.Rendering;

namespace Economy.Infrastructure.TimeZones
{
    public class TimeZoneService : ITimeZoneService
    {
        public ICollection<SelectListItem> GetAllTimeZones()
        {
            var allTimeZones = TimeZoneInfo.GetSystemTimeZones();

            var selectItems = allTimeZones.Select(tz => new SelectListItem
            {
                Value = tz.Id,
                Text = tz.DisplayName
            }).ToList();

            return selectItems;
        }
    }
}
