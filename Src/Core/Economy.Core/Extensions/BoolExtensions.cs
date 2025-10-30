namespace Economy.Core.Extensions
{
    public static class BoolExtensions
    {
        public static string ToTurkishYesNo(this bool value)
        {
            return value ? "Evet" : "Hayır";
        }
        public static string GetEvetHayirText(this bool value)
        {
            return value ? "Evet" : "Hayır";
        }
        public static string GetAktifPasifText(this bool value)
        {
            return value ? "Aktif" : "Pasif";
        }
        public static string GetYesNoText(this bool value)
        {
            return value ? "Var" : "Yok";
        }
    }
}
