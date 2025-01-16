using Economy.Domain.BaseEntities;

namespace Economy.Domain.Entites.EntitySlides
{
	public class AppSlideTranslation : BaseEntity<int>
    {
        public int AppSlideId { get; set; }

        /// <summary>
        /// Dil kodu (örneğin: "en", "tr").
        /// </summary>
        public string LanguageCode { get; set; }

        /// <summary>
        /// Slide öğesinin adını içerir.
        /// </summary>
        public string Title { get; set; }
        public string? Content { get; set; }

        /// <summary>
        /// Harici URL'yi içerir.
        /// </summary>
        public string URL { get; set; } // Harici bağlantı için URL

        /// <summary>
        /// Harici bağlantı olup olmadığını belirtir.
        /// </summary>
        public bool IsExternal { get; set; }


        public string ImageUrl { get; set; }
        /// <summary>
        /// İlişkilendirilmiş Slide Öğesi.
        /// </summary>
        public virtual AppSlide AppSlide { get; set; } = new AppSlide();
    }
}
