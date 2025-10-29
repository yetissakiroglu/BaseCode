using Economy.Core.Enums;

namespace Economy.Core.Extensions
{
    public static class BlockTypeExtensions
    {
        public static string GetSharedJson(this BlockType type)
        {
            return type switch
            {
                BlockType.Hero => """{"backgroundUrl":"/media/hero.jpg","verticalAlign":"center"}""",
                BlockType.Text => """{}""",
                BlockType.ImageGallery => """{"mode":"grid","imageUrls":[]}""",
                BlockType.AmenityGroup => """{}""",
                _ => """{}"""
            };
        }

        public static string GetLocalizedJson(this BlockType type)
        {
            return type switch
            {
                BlockType.Hero =>
                    """{"heading":"Başlık","subHeading":"Alt başlık","buttonText":"Devam","buttonUrl":"/"}""",

                BlockType.Text =>
                    """{"heading":"Bölüm","bodyHtml":"<p>Metin…</p>"}""",

                BlockType.ImageGallery =>
                    """{}""",

                BlockType.AmenityGroup =>
                    """{"groupTitle":"Oda Olanakları","amenities":["Ücretsiz Wi-Fi","Klima","TV"]}""",

                _ => """{}"""
            };
        }
    }
}
