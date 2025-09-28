namespace Economy.Core.Enums
{
    // ======================
    // ENUMS (seninle netleştirdiklerimiz)
    // ======================

    public enum ContentItemType : short
    {
        Page = 1,       // dil+slug ile adreslenir: odalar, kampanyalar, otelimiz, KVKK...
        Snippet = 2,    // küçük tekrar içeriği: PET_POLICY_NOTE, MINI_BAR_NOTE (code ile)
        Block = 3       // bir içeriğin içinde render edilen modül
    }

    public enum ContentOwnerType : byte
    {
        None = 0,       // Page & Snippet
        Content = 1,    // bir Page içindeki Block vb.
        Room = 2,       // opsiyonel harici varlık
        Campaign = 3    // opsiyonel harici varlık
    }

    public enum BlockTemplate : short
    {
        RichText = 1,
        ImageText = 2,
        Hero = 3,
        Gallery = 4,
        IncludeSnippet = 5,
        PageList = 6,       // genel kart listesi (her tip sayfa)
        RoomList = 7,       // oda kart listesi
        CampaignList = 8    // kampanya kart listesi
    }

    public enum MediaOwnerType : byte
    {
        Content = 1,    // ContentItem (Page/Block) medyası
        Room = 2,       // opsiyonel
        Campaign = 3    // opsiyonel
    }

}
