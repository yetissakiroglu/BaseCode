using Microsoft.EntityFrameworkCore;

namespace Economy.Application.Repositories
{
    public interface IRepository<T> where T : class, new()
    {
        DbSet<T> Table { get; }
    }
}
