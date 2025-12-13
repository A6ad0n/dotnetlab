using Microsoft.EntityFrameworkCore;
using PizzaApp.DataAccess.Context;
using PizzaApp.DataAccess.Entities;
using PizzaApp.DataAccess.Entities.Primitives;

namespace PizzaApp.DataAccess.Repository.DiscountRepository;

public class DiscountRepository(IDbContextFactory<PizzaAppDbContext> contextFactory)
    : Repository<DiscountEntity>(contextFactory), IDiscountRepository
{
    public async Task<DiscountEntity?> GetByIdWithDetailsAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Discounts
            .Include(m => m.Status)
            .Include(m => m.MenuItems)
                .ThenInclude(md => md.MenuItem)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
    
    public async Task<DiscountEntity?> GetByGuidWithDetailsAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Discounts
            .Include(m => m.Status)
            .Include(m => m.MenuItems)
            .ThenInclude(md => md.MenuItem)
            .FirstOrDefaultAsync(m => m.ExternalId == guid);
    }
    
    public async Task<DiscountEntity?> GetByIdWithStatusAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Discounts
            .Include(d => d.Status)
            .FirstOrDefaultAsync(d => d.Id == id);
    }
    
    public async Task<DiscountEntity?> GetByGuidWithStatusAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Discounts
            .Include(d => d.Status)
            .FirstOrDefaultAsync(d => d.ExternalId == guid);
    }
    
    public async Task<List<DiscountEntity>> GetAllWithDetailsAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Discounts
            .Include(d => d.Status)
            .Include(d => d.MenuItems)
                .ThenInclude(ur => ur.MenuItem)
            .ToListAsync();
    }
        
    public async Task<List<DiscountEntity>> GetDiscountsByStatusAsync(Status status)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Discounts
            .Include(d => d.Status)
            .Where(d => d.Status.Name == status)
            .ToListAsync();
    }
    
    public async Task<List<DiscountEntity>> GetDiscountsByStatusIdAsync(int statusId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Discounts
            .Include(d => d.Status)
            .Where(d => d.StatusId == statusId)
            .ToListAsync();
    }
    
    public async Task<List<DiscountEntity>> GetDiscountsByStatusGuidAsync(Guid statusGuid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Discounts
            .Include(d => d.Status)
            .Where(d => d.Status.ExternalId == statusGuid)
            .ToListAsync();
    }

    
    public async Task<(List<DiscountEntity> Discounts, int TotalCount)> GetDiscountsPagedAsync(
        int pageNumber, 
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        bool ascending = true)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        var query = context.Discounts.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(d => d.Name.Contains(searchTerm));
        }
        
        query = sortBy?.ToLower() switch
        {
            "name" => ascending 
                ? query.OrderBy(d => d.Name) 
                : query.OrderByDescending(d => d.Name),
            "discountPercentage" => ascending 
                ? query.OrderBy(d => d.DiscountPercentage) 
                : query.OrderByDescending(d => d.DiscountPercentage),
            "validTo" => ascending 
                ? query.OrderBy(d => d.ValidTo) 
                : query.OrderByDescending(d => d.ValidTo),
            _ => ascending 
                ? query.OrderBy(d => d.Id) 
                : query.OrderByDescending(d => d.Id)
        };
        
        var totalCount = await query.CountAsync();
        
        var discounts = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (discounts, totalCount);
    }
    
    
    public async Task<int> GetDiscountsCountAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Discounts.CountAsync();
    }


    public async Task<bool> ExistsStatusAsync(int statusId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Discounts.AnyAsync(d => d.StatusId == statusId);
    }
    public async Task<bool> ExistsStatusAsync(Guid statusGuid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Discounts
            .Include(d => d.Status)
            .AnyAsync(d => d.Status.ExternalId == statusGuid);
    }
    public async Task<List<StatusEntity>> GetAllStatusesAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Statuses.ToListAsync();
    }

    public async Task UpdateDiscountStatusAsync(DiscountEntity discount, int statusId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        discount.StatusId = statusId;
        discount.Status = await context.Statuses.FindAsync(statusId);

        context.Discounts.Update(discount);
        await context.SaveChangesAsync();
    }
    
    public async Task UpdateDiscountStatusAsync(DiscountEntity discount, Guid statusGuid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        var statusId = await context.Statuses
            .Where(s => s.ExternalId == statusGuid)
            .Select(s => s.Id)
            .FirstOrDefaultAsync();

        discount.StatusId = statusId;
        discount.Status = await context.Statuses.FindAsync(statusId);

        context.Discounts.Update(discount);
        await context.SaveChangesAsync();
    }

}