namespace Economy.Infrastructure.Providers
{
    public class LanguageProvider : ILanguageProvider
    {

        private string _currentLanguage = "tr";
        // Geçerli dil kodu özelliği
        public string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _currentLanguage = value;
                }
            }
        }
    }
}
