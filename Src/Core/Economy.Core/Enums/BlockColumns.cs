namespace Economy.Core.Enums
{
    /// <summary>
    /// Bootstrap grid sistemine göre blok genişliklerini belirtir.
    /// </summary>
    using System.ComponentModel.DataAnnotations;

    public enum BlockColumns
    {
        [Display(Name = "1 Sütun Genişlik (col-1)")]
        One = 1,

        [Display(Name = "2 Sütun Genişlik (col-2)")]
        Two = 2,

        [Display(Name = "3 Sütun Genişlik (col-3)")]
        Three = 3,

        [Display(Name = "4 Sütun Genişlik (col-4)")]
        Four = 4,

        [Display(Name = "5 Sütun Genişlik (col-5)")]
        Five = 5,

        [Display(Name = "6 Sütun Genişlik (col-6)")]
        Six = 6,

        [Display(Name = "7 Sütun Genişlik (col-7)")]
        Seven = 7,

        [Display(Name = "8 Sütun Genişlik (col-8)")]
        Eight = 8,

        [Display(Name = "9 Sütun Genişlik (col-9)")]
        Nine = 9,

        [Display(Name = "10 Sütun Genişlik (col-10)")]
        Ten = 10,

        [Display(Name = "11 Sütun Genişlik (col-11)")]
        Eleven = 11,

        [Display(Name = "12 Sütun (Tam Genişlik - col-12)")]
        Twelve = 12
    }

}
