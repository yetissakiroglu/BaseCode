using Economy.Domain.Entites.EntityAppLanguage;
using Economy.Persistence.Contexts;

namespace Economy.Persistence
{
    public class LanguageService
    {
        private readonly AppDbContext _dbContext;

        public LanguageService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<AppLanguage> GetSupportedLanguages()
        {
            return _dbContext.AppLanguages.Where(l => l.IsActive).ToList();
        }
    }
}
