using Economy.Domain.Entites.EntityAppSettings;

namespace Economy.Application.Dtos.WebSettingDtos
{
    public record AppSettingDto
	{
		public int Id { get; set; } // Zorunlu
		public bool IsCached { get; set; } // Ayarın cache'de tutulup tutulmadığını belirtir
		public TimeSpan? CacheDuration { get; set; } // Cache'de tutulma süresi (nullable)
		public string SiteLogo { get; set; } // Logo dosya yolu (zorunlu)
		public string? SiteFavicon { get; set; } // Favicon dosya yolu (isteğe bağlı)
		public string? FacebookUrl { get; set; } // Sosyal medya (isteğe bağlı)
		public string? TwitterUrl { get; set; }
		public string? InstagramUrl { get; set; }
		public string? ContactEmail { get; set; } // İletişim e-postası (isteğe bağlı)
		public string? ContactPhone { get; set; } // İletişim telefonu (isteğe bağlı)
		public string? Address { get; set; } // Fiziksel adres (isteğe bağlı)
		public bool IsMaintenanceMode { get; set; } // Bakım modu açık mı?
		public string? MaintenanceMessage { get; set; } // Bakım mesajı (isteğe bağlı)

		// Entity'den DTO'ya dönüşüm metodu
		public static AppSettingDto FromEntity(AppSetting entity)
		{
			return new AppSettingDto
			{
				Id = entity.Id,
				//IsCached = entity.IsCached,
				//CacheDuration = entity.CacheDuration,
				SiteLogo = entity.SiteLogo,
				SiteFavicon = entity.SiteFavicon,
				//FacebookUrl = entity.FacebookUrl,
				//TwitterUrl = entity.TwitterUrl,
				//InstagramUrl = entity.InstagramUrl,
				//ContactEmail = entity.ContactEmail,
				//ContactPhone = entity.ContactPhone,
				//Address = entity.Address,
				IsMaintenanceMode = entity.IsMaintenanceMode,
				MaintenanceMessage = entity.MaintenanceMessage
			};
		}

		// Listeleme metodu
		public static List<AppSettingDto> List(List<AppSetting> entities)
		{
			return entities.Select(entity => FromEntity(entity)).ToList();
		}
	}

	// CreateModel Özellikleri
	//public static AppSetting CreateEntity(string title, string content, string languageCode, bool isDefault)
	//{
	//    var translation = new AppSettingTranslation
	//    {
	//        Title = title,
	//        Content = content,
	//        LanguageCode = languageCode
	//    };

	//    return new AppSetting
	//    {
	//        Translations = new List<AppSettingTranslation> { translation }
	//    };
	//}

	//// EditModel Özellikleri
	//public static AppSetting EditEntity(int id, string title, string content, string languageCode, bool isDefault)
	//{
	//    var translation = new AppSettingTranslation
	//    {
	//        AppSettingId = id, // İlgili AppSetting'e ait
	//        Title = title,
	//        Content = content,
	//        LanguageCode = languageCode
	//    };

	//    return new AppSetting
	//    {
	//        Id = id,
	//        Translations = new List<AppSettingTranslation> { translation }
	//    };
	//}

}
