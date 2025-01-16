using Economy.Persistence.Services;
using System.Linq.Expressions;

namespace Economy.Application.Services.BaseServices
{
	public interface IService<T, TId> where T : class
	{
		Task<ServiceResult<T>> GetForReadAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);
		Task<ServiceResult<T>> GetForEditAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes);

		Task<ServiceResult<List<T>>> WhereForReadAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes);
        Task<ServiceResult<List<T>>> WhereForReadDeletedAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes);

        Task<ServiceResult<List<T>>> WhereForReadNotDeleteAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes);
        
        Task<ServiceResult<List<T>>> WhereForEditAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes);

		Task<ServiceResult<bool>> AnyAsync(Expression<Func<T, bool>>? filters = null);

		Task<ServiceResult<T>> AddAsync(T entity);
		Task<ServiceResult<T>> UpdateAsync(T entity);
		Task<ServiceResult<T>> DeleteAsync(T entity);


	}
}
