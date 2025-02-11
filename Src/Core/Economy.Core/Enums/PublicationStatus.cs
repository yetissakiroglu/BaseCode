using System.ComponentModel;

namespace Economy.Domain.Enums
{
    public enum PublicationStatus
    {

        [Description("Taslak")]
        Draft = 0,

        [Description("Yayında")]
        Published = 1,

        [Description("Arşivlenmiş")]
        Archived = 2
    }
}
