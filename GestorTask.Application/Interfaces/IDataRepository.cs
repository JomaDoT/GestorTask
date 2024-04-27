using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GestorTask.Application.Interfaces;

public interface IDataRepository
{
}

public interface IDataRepository<T> : IDataRepository
    where T : class, new()
{
    Task<T> GetEntityAsync(int id);

    Task<T> AddAsync(T entity);

    Task<T> UpdateAsync(T entity);
    Task<T> RemoveAsync(T entity);

    Task<IEnumerable<TResult>> GetAllAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> transform = null, Expression<Func<T, bool>> filter = null, string sortExpression = null, List<string> relation = null);
    Task<TResult> GetAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> transform, Expression<Func<T, bool>> filter = null, string sortExpression = null, List<string> relation = null);
    Task<bool> ExistsAsync<TResult>(Func<IQueryable<T>, IQueryable<TResult>> transform, Expression<Func<T, bool>> filter = null);
}