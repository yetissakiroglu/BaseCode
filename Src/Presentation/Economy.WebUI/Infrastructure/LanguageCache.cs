namespace Economy.Web.UI.Infrastructure
{
    public class LanguageCache
    {
        private readonly HashSet<string> _shorts = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _fulls = new(StringComparer.OrdinalIgnoreCase);
        public void Set(IEnumerable<(string Short, string Full)> langs) { _shorts.Clear(); _fulls.Clear(); foreach (var (s, f) in langs) { _shorts.Add(s); _fulls.Add(f); } }
        public bool IsShort(string s) => _shorts.Contains(s); public bool IsFull(string s) => _fulls.Contains(s);
    }

}
