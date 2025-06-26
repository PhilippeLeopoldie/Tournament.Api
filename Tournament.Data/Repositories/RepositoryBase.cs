using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Tournament.Infrastructure.Data;

namespace Tournament.Infrastructure.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected TournamentApiContext _context;
    protected DbSet<T> _dbSet;

    public RepositoryBase(TournamentApiContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public void Create(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public IQueryable<T> FindAll(bool trackChanges = false)
    {
        return trackChanges ? _dbSet : _dbSet.AsNoTracking();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition, bool trackChanges = false)
    {
        return trackChanges ? _dbSet.Where(condition) : _dbSet.Where(condition).AsNoTracking();
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}
