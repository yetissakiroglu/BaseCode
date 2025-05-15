using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntityAppSettings
{
    public class AppSettingReservationLink : BaseEntity<int>
    {
        public string Url { get; set; }

    }
}
