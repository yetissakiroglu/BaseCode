namespace Economy.Caching.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CacheAttribute : Attribute
    {
        public int Duration { get; set; } = 30;
    }
}
