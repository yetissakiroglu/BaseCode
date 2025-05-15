namespace Economy.Panel.UI.Models.ReservationNumberViewModels
{
    public class AppSettingReservationNumberCreateViewModel
    {
        public string CountryCode { get; set; }  // Ülke kodu, örn: "+90", "+1"
        public string Number { get; set; }       // Telefon numarasının geri kalanı, örn: "5551112233"
        public bool IsPrimary { get; set; }      // Birincil numara mı?
    }
}
