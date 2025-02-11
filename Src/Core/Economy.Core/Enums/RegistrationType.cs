using System.ComponentModel;

namespace Economy.Domain.Enums
{
    public enum RegistrationType
    {
        [Description("RegistrationBySelf")]
        RegistrationBySelf = 0,

        [Description("RegistrationByAdmin")]
        RegistrationByAdmin = 1
    }
}
