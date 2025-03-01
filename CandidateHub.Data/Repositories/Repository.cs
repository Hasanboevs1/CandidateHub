﻿using CandidateHub.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CandidateHub.Data.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual IQueryable<T> GetAll(Expression<Func<T, bool>> expression, string[] includes = null, bool isTracking = true)
    {
        var query = expression is null ? _dbSet : _dbSet.Where(expression);

        if (includes != null)
        {
            foreach (var include in includes)
            {
                if (!string.IsNullOrEmpty(include))
                {
                    query = query.Include(include);
                }
            }
        }

        if (!isTracking)
        {
            query = query.AsNoTracking().AsSplitQuery();
        }

        return query;
    }

    public virtual async ValueTask<T> GetAsync(Expression<Func<T, bool>> expression, bool isTracking = true, string[] includes = null) =>
        await GetAll(expression, includes, isTracking).FirstOrDefaultAsync();

    public async ValueTask<T> CreateAsync(T entity) =>
        (await _context.AddAsync(entity)).Entity;

    public async ValueTask<bool> DeleteAsync(long id)
    {
        var entity = await GetAsync(e => EF.Property<long>(e, "Id") == id);

        if (entity == null)
        {
            return false;
        }

        _dbSet.Remove(entity);

        return true;
    }

    public T Update(T entity) =>
        _dbSet.Update(entity).Entity;

    public async ValueTask SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
