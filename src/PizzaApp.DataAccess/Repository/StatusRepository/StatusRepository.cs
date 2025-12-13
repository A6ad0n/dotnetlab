using Microsoft.EntityFrameworkCore;
using PizzaApp.DataAccess.Context;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.DataAccess.Repository.StatusRepository;

public class StatusRepository(IDbContextFactory<PizzaAppDbContext> contextFactory)
    : Repository<StatusEntity>(contextFactory), IStatusRepository
{
     public async Task<StatusEntity?> GetByIdWithDetailsAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Statuses
            .Include(s => s.MenuItems)
            .Include(s => s.Discounts)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    
    public async Task<StatusEntity?> GetByGuidWithDetailsAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Statuses
            .Include(s => s.MenuItems)
            .Include(s => s.Discounts)
            .FirstOrDefaultAsync(s => s.ExternalId == guid);
    }
    
    public async Task<StatusEntity?> GetByIdWithDiscountsAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Statuses
            .Include(s => s.Discounts)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    
    public async Task<StatusEntity?> GetByIdWithMenuItemsAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Statuses
            .Include(s => s.MenuItems)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    
    public async Task<StatusEntity?> GetByIdWithOrdersAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Statuses
            .Include(s => s.Orders)
                .ThenInclude(o => o.OrderItems)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    
    public async Task<StatusEntity?> GetByIdWithAllDataAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Statuses
            .Include(s => s.MenuItems)
            .Include(s => s.Discounts)
            .Include(s => s.Orders)
                .ThenInclude(o => o.OrderItems)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
    
    public async Task<StatusEntity?> GetByGuidWithDiscountsAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Statuses
            .Include(s => s.Discounts)
            .FirstOrDefaultAsync(s => s.ExternalId == guid);
    }
    
    public async Task<StatusEntity?> GetByGuidWithMenuItemsAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Statuses
            .Include(s => s.MenuItems)
            .FirstOrDefaultAsync(s => s.ExternalId == guid);
    }
    
    public async Task<StatusEntity?> GetByGuidWithOrdersAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Statuses
            .Include(s => s.Orders)
            .ThenInclude(o => o.OrderItems)
            .FirstOrDefaultAsync(s => s.ExternalId == guid);
    }
    
    public async Task<StatusEntity?> GetByGuidWithAllDataAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Statuses
            .Include(s => s.MenuItems)
            .Include(s => s.Discounts)
            .Include(s => s.Orders)
            .ThenInclude(o => o.OrderItems)
            .FirstOrDefaultAsync(s => s.ExternalId == guid);
    }
    
    public async Task<List<StatusEntity>> GetAllWithDetailsAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Statuses
            .Include(s => s.MenuItems)
            .Include(s => s.Discounts)
            .ToListAsync();
    }
    
    public async Task<(List<StatusEntity> Statuses, int TotalCount)> GetStatusesPagedAsync(
        int pageNumber, 
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        bool ascending = true)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        var query = context.Statuses.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(s =>
                s.Name.ToString().Contains(searchTerm));
        }
        
        query = sortBy?.ToLower() switch
        {
            "name" => ascending 
                ? query.OrderBy(s => s.Name.ToString()) 
                : query.OrderByDescending(s => s.Name.ToString()),
            _ => ascending 
                ? query.OrderBy(s => s.Id) 
                : query.OrderByDescending(s => s.Id)
        };
        
        var totalCount = await query.CountAsync();
        
        var statuses = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (statuses, totalCount);
    }
    
    
    public async Task<int> GetStatusesCountAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.Statuses.CountAsync();
    }

}