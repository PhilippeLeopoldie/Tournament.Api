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
    protected TournamentApiContext Context { get; set; }
    protected DbSet<T> DbSet { get; set; }

    public RepositoryBase(TournamentApiContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }

    public void Create(T entity)
    {
        DbSet.Add(entity);
    }

    public void Delete(T entity)
    {
        DbSet.Remove(entity);
    }

    public IQueryable<T> FindAll(bool trackChanges = false)
    {
        return trackChanges ? DbSet : DbSet.AsNoTracking();
    }

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition, bool trackChanges = false)
    {
        return trackChanges ? DbSet.Where(condition) : DbSet.Where(condition).AsNoTracking();
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }
}
