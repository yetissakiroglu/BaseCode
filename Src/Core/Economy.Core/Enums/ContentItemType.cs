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
     

 

}
