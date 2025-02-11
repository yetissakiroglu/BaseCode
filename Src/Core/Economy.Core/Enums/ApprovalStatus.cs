using System.ComponentModel;

namespace Economy.Domain.Enums
{
    public enum ApprovalStatus
    {
        [Description("Onay bekliyor")]
        Pending = 0,

        [Description("Onaylandı")]
        Approved = 1,

        [Description("Reddedildi")]
        Rejected = 2

    }
}
