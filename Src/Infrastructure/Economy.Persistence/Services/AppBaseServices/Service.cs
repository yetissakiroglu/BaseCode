using Economy.Application.Repositories;
using Economy.Application.Services.BaseServices;
using Economy.Application.UnitOfWorks;
using Economy.Domain.BaseEntities;
using Economy.Infrastructure.DateFormats;
using Economy.Persistence.Contexts;
using Economy.Persistence.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;

namespace Economy.Persistence.Services.BaseServices
{
    public class Service<T, TId> : IService<T, TId> where T : class, IHasId<TId>, ISoftDelete, new()
    {
        private readonly IEntityRepository<T, TId> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public Service(IEntityRepository<T, TId> readRepository, IUnitOfWork unitOfWork)
        {
            _repository = readRepository;
            _unitOfWork = unitOfWork;
        }



        public async Task<ServiceResult<T>> AddAsync(T entity)
        {
            if (entity != null)
            {
                await _repository.AddAsync(entity);
                await _unitOfWork.CommitAsync();
                return ServiceResult<T>.Success(entity, HttpStatusCode.OK);
            }
            else
            {
                throw new Exception("Unable to process, entity is null");
            }
        }

        public async Task<ServiceResult<bool>> AnyAsync(Expression<Func<T, bool>>? filters = null)
        {
            var exists = await _repository.Table.AnyAsync(filters);
            return ServiceResult<bool>.Success(exists, HttpStatusCode.OK);
        }

        public async Task<ServiceResult<T>> DeleteAsync(T entity)
        {
            if (entity != null)
            {
                if (entity is ISoftDelete softDeleteEntity)
                {
                    softDeleteEntity.IsDeleted = true;
                    await _repository.UpdateAsync(entity);
                }
                else
                {
                    await _repository.DeleteAsync(entity);
                }
                await _unitOfWork.CommitAsync();
                return ServiceResult<T>.Success(entity, HttpStatusCode.OK);
            }
            else
            {
                throw new Exception("Unable to process, entity is null");
            }
        }

        public async Task<ServiceResult<T>> GetForEditAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _repository.Table.AsQueryable(); // Varsayılan sorgu

            // Include ifadelerini ekle
            if (includes != null)
            {
                foreach (var includeExpression in includes)
                {
                    query = query.Include(includeExpression);
                }
            }

            // Filtreleri uygula
            if (filters is not null)
            {
                query = query.Where(filters);
            }

            var result = await query.FirstOrDefaultAsync();

            return ServiceResult<T>.Success(result, HttpStatusCode.OK);
        }

        public async Task<ServiceResult<T>> GetForReadAsync(Expression<Func<T, bool>>? filters = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _repository.Table.AsNoTracking().AsQueryable(); // Varsayılan sorgu

            // Include ifadelerini ekle
            if (includes != null)
            {
                foreach (var includeExpression in includes)
                {
                    query = query.Include(includeExpression);
                }
            }

            // Filtreleri uygula
            if (filters is not null)
            {
                query = query.Where(filters);
            }

            var result = await query.FirstOrDefaultAsync(); // İlk sonucu al
            if (result != null)
            {
                await _auditColumnTransformer.TransformAsync(result, _context, _dateFormatConfiguration);
            }

            return ServiceResult<T>.Success(result, HttpStatusCode.OK);
        }

        public async Task<ServiceResult<T>> UpdateAsync(T entity)
        {
            if (entity != null)
            {
                await _repository.UpdateAsync(entity); // Varlık güncelleniyor
                await _unitOfWork.CommitAsync(); // Değişiklikler kaydediliyor
                return ServiceResult<T>.Success(entity, HttpStatusCode.OK);
            }
            else
            {
                throw new Exception("Unable to process, entity is null");
            }
        }

        public async Task<ServiceResult<List<T>>> WhereForEditAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _repository.Table.AsQueryable(); // Varsayılan sorgu

            // Include ifadelerini ekle
            if (includes != null)
            {
                foreach (var includeExpression in includes)
                {
                    query = query.Include(includeExpression);
                }
            }

            // Filtreleri uygula
            if (filter is not null)
            {
                query = query.Where(filter);
            }

            var result = await query.ToListAsync(); // Filtrelenmiş listeyi al
            return ServiceResult<List<T>>.Success(result, HttpStatusCode.OK);

        }

        public async Task<ServiceResult<List<T>>> WhereForReadAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _repository.Table.AsQueryable(); // Varsayılan sorgu

            // Include ifadelerini ekle
            if (includes != null)
            {
                foreach (var includeExpression in includes)
                {
                    query = query.Include(includeExpression);
                }
            }

            // Filtreleri uygula
            if (filter is not null)
            {
                query = query.Where(filter);
            }

            var result = await query.ToListAsync(); // Filtrelenmiş listeyi al
            return ServiceResult<List<T>>.Success(result, HttpStatusCode.OK);
        }

        public async Task<ServiceResult<List<T>>> WhereForReadDeletedAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _repository.Table.AsQueryable().ApplyIsDeletedFilter(true); // Varsayılan sorgu

            // Include ifadelerini ekle
            if (includes != null)
            {
                foreach (var includeExpression in includes)
                {
                    query = query.Include(includeExpression);
                }
            }

            // Filtreleri uygula
            if (filter is not null)
            {
                query = query.Where(filter);
            }

            var result = await query.ToListAsync(); // Filtrelenmiş listeyi al
            return ServiceResult<List<T>>.Success(result, HttpStatusCode.OK);
        }

        public async Task<ServiceResult<List<T>>> WhereForReadNotDeleteAsync(Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _repository.Table.AsQueryable().ApplyIsDeletedFalseFilter(true); // Varsayılan sorgu

            // Include ifadelerini ekle
            if (includes != null)
            {
                foreach (var includeExpression in includes)
                {
                    query = query.Include(includeExpression);
                }
            }

            // Filtreleri uygula
            if (filter is not null)
            {
                query = query.Where(filter);
            }

            var result = await query.ToListAsync(); // Filtrelenmiş listeyi al
            return ServiceResult<List<T>>.Success(result, HttpStatusCode.OK);
        }
    }

}
