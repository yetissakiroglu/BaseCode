namespace Economy.Panel.UI.Models.WhatsappLineViewModels
{
    public class AppSettingWhatsappLineViewModel
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }  // Ülke kodu, örn: "+90", "+1"
        public string Number { get; set; }       // Telefon numarasının geri kalanı, örn: "5551112233"
        public bool IsPrimary { get; set; }      // Birincil numara mı?
    }
}
