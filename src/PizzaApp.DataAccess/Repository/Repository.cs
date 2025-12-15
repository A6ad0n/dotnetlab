using PizzaApp.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using PizzaApp.DataAccess.Context;

namespace PizzaApp.DataAccess.Repository;

public class Repository<T>(IDbContextFactory<PizzaAppDbContext> contextFactory) : IRepository<T>
    where T : class, IBaseEntity
{
    public IQueryable<T> GetAll()
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Set<T>();
    }
    
    public async Task<IQueryable<T>> GetAllAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return context.Set<T>();
    }


    public T? GetById(int id)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Set<T>().FirstOrDefault(x => x.Id == id);
    }
    
    public async Task<T?> GetByIdAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
    }
    
    public T? GetByGuid(Guid guid)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.Set<T>().FirstOrDefault(x => x.ExternalId == guid);
    }
    
    public async Task<T?> GetByGuidAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Set<T>().FirstOrDefaultAsync(x => x.ExternalId == guid);
    }
    

    public T Save(T entity)
    {
        using var context = _contextFactory.CreateDbContext();
        if (context.Set<T>().Any(x => x.Id == entity.Id))
        {
            entity.ModificationTime = DateTime.UtcNow;
            var result = context.Set<T>().Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
            return result.Entity;
        }
        else
        {
            entity.ExternalId = Guid.NewGuid();
            entity.CreationTime = DateTime.UtcNow;
            entity.ModificationTime = entity.CreationTime;
            var result = context.Set<T>().Add(entity);
            context.SaveChanges();
            return result.Entity;
        }
    }

    public async Task<T> SaveAsync(T entity)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var existing = await context.Set<T>().FindAsync(entity.Id);
        if (existing != null)
        {
            context.Entry(existing).CurrentValues.SetValues(entity);
            existing.ModificationTime = DateTime.UtcNow;
            await context.SaveChangesAsync();
            return existing;
        }

        entity.ExternalId = Guid.NewGuid();
        entity.CreationTime = DateTime.UtcNow;
        entity.ModificationTime = entity.CreationTime;

        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    
    public bool Delete(int id)
    {
        using var context =  _contextFactory.CreateDbContext();
        var entity = context.Set<T>().Find(id);
        if (entity == null)
        {
            return false;
        }

        context.Set<T>().Remove(entity);
        var affectedRows = context.SaveChanges();
        return affectedRows > 0;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var entity = await context.Set<T>().FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        context.Set<T>().Remove(entity);
        var affectedRows = await context.SaveChangesAsync();
        return affectedRows > 0;
    }
    
    public bool Delete(Guid guid)
    {
        using var context =  _contextFactory.CreateDbContext();
        var entity = context.Set<T>()
            .FirstOrDefault(e => e.ExternalId == guid);
    
        if (entity == null)
        {
            return false;
        }

        context.Set<T>().Remove(entity);
        var affectedRows = context.SaveChanges();
        return affectedRows > 0;
    }
    
    public async Task<bool> DeleteAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var entity = await context.Set<T>()
            .FirstOrDefaultAsync(e => e.ExternalId == guid);
    
        if (entity == null)
        {
            return false;
        }

        context.Set<T>().Remove(entity);
        var affectedRows = await context.SaveChangesAsync();
        return affectedRows > 0;
    }
    
    protected readonly IDbContextFactory<PizzaAppDbContext> _contextFactory = contextFactory;
}