using System.ComponentModel.DataAnnotations;

namespace Economy.Core.Enums
{
    public enum GroupTypes
    {
        [Display(Name = "Default Gösterimi")]
        Default = 0,
        [Display(Name = "Odalarımız Gösterimi")]
        RoomView = 1,

    }
}
