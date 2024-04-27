using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using GestorTask.Application.Interfaces;

namespace GestorTask.Application.Implementation;
public abstract class BaseRepository<TEntity, U> : IDataRepository<TEntity>
    where TEntity : class, new()
    where U : DbContext
{
    protected readonly U _Context;
    private readonly DbSet<TEntity> _DbSet;

    protected BaseRepository(U context)
    {
        _Context = context;
        _DbSet = _Context.Set<TEntity>();
    }
    public virtual async Task<TEntity> GetEntityAsync(int id)
    {
        return await _DbSet.FindAsync(id, GenerateToken(3000));
    }
    public virtual async Task<TEntity> AddAsync(TEntity entity)
    {
        await _Context.Set<TEntity>().AddAsync(entity);

        await _Context.SaveChangesAsync(GenerateToken(4000));

        return entity;
    }

    public virtual async Task<TEntity> RemoveAsync(TEntity entity)
    {
        _DbSet.Attach(entity);

        _Context.Entry<TEntity>(entity).State = EntityState.Deleted;

        await _Context.SaveChangesAsync(GenerateToken(3000));

        return entity;

    }

    public virtual async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _DbSet.Attach(entity);

        _Context.Entry(entity).State = EntityState.Modified;

        await _Context.SaveChangesAsync(GenerateToken(5000));

        return entity;
    }
    public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null, string sortExpression = null, List<string> relation = null)
    {
        var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

        if (relation is not null)
        {
            foreach (var item in relation)
                query = query.Include(item).AsSplitQuery();
        }

        return await transform(query).ToListAsync(GenerateToken(4000));
    }

    public async Task<TResult> GetAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null, string sortExpression = null, List<string> relation = null)
    {
        var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);
        if (relation is not null)
        {
            foreach (var item in relation)
                query = query.Include(item).AsSplitQuery();
        }
        return await transform(query).FirstOrDefaultAsync(GenerateToken(4000));
    }

    public async Task<bool> ExistsAsync<TResult>(Func<IQueryable<TEntity>, IQueryable<TResult>> transform, Expression<Func<TEntity, bool>> filter = null)
    {
        var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

        return await transform(query).AnyAsync(GenerateToken(3000));
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter, string sortExpression = null, string relation = null)
    {
        var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);
        var query1 = relation == null ? query : query.Include(relation);

        return await query1.ToListAsync(GenerateToken(4000));
    }

    public async Task<int> CountAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> transform, Expression<Func<TEntity, bool>> filter = null)
    {
        var query = filter == null ? _DbSet.AsNoTracking() : _DbSet.AsNoTracking().Where(filter);

        return await transform(query).CountAsync(GenerateToken(3000));

    }
    private static CancellationToken GenerateToken(int duration)
    {
        CancellationTokenSource tks = new(duration);
        CancellationToken tk = tks.Token;

        return tk;
    }

}