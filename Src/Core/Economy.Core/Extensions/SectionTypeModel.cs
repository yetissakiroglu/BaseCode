namespace Economy.Core.Extensions
{
    public class SectionTypeModel
    {
        public string SectionHeading { get; set; } = string.Empty;
        public string SectionDescription { get; set; } = string.Empty;
        public List<int> PageIDs { get; set; } = new();
    }
}
