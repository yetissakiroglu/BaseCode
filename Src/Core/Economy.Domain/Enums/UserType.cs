using System.ComponentModel;

namespace Economy.Domain.Enums
{
    public enum UserType
    {
        [Description("Internal")]
        Internal = 0,
        [Description("Editor")]
        Editor = 1
    }
}
