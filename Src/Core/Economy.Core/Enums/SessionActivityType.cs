using System.ComponentModel;

namespace Economy.Domain.Enums
{
    public enum SessionActivityType
    {
        [Description("Login")]
        Login = 0,
        [Description("Logout")]
        Logout = 1
    }
}
