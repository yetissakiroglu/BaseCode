using Economy.Core.UnitOfWorks;
using Economy.Persistence.Contexts;

namespace Economy.Persistence.UnitOfWorks
{
    public class UnitOfWork(AppDbContext context) : IUnitOfWork
    {
        public Task<int> CommitAsync()
        {
            return context.SaveChangesAsync();
        }
    }
}
