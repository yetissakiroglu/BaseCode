using Microsoft.EntityFrameworkCore;

namespace Economy.Core.Repositories
{
    public interface IRepository<T> where T : class, new()
    {
        DbSet<T> Table { get; }
    }
}
