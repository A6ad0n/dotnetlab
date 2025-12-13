using Microsoft.EntityFrameworkCore;
using PizzaApp.DataAccess.Context;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.DataAccess.Repository.CategoryRepository;

public class CategoryRepository(IDbContextFactory<PizzaAppDbContext> contextFactory)
    : Repository<MenuCategoryEntity>(contextFactory), ICategoryRepository
{
     public async Task<MenuCategoryEntity?> GetByIdWithDetailsAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuCategories
            .Include(c => c.MenuItems)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<MenuCategoryEntity?> GetByGuidWithDetailsAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuCategories
            .Include(c => c.MenuItems)
            .FirstOrDefaultAsync(c => c.ExternalId == guid);
    }
    
 
    public async Task<MenuCategoryEntity?> GetByIdWithAllDataAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuCategories
            .Include(c => c.MenuItems)
                .ThenInclude(mi => mi.Status)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<MenuCategoryEntity?> GetByGuidWithAllDataAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuCategories
            .Include(c => c.MenuItems)
                .ThenInclude(mi => mi.Status)
            .FirstOrDefaultAsync(c => c.ExternalId == guid);
    }
    
    public async Task<List<MenuCategoryEntity>> GetAllWithDetailsAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.MenuCategories
            .Include(c => c.MenuItems)
            .ToListAsync();
    }
    
    public async Task<(List<MenuCategoryEntity> Categories, int TotalCount)> GetCategoriesPagedAsync(
        int pageNumber, 
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        bool ascending = true)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        var query = context.MenuCategories.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(c =>
                c.Name.ToString().Contains(searchTerm));
        }
        
        query = sortBy?.ToLower() switch
        {
            "name" => ascending 
                ? query.OrderBy(c => c.Name.ToString()) 
                : query.OrderByDescending(c => c.Name.ToString()),
            _ => ascending 
                ? query.OrderBy(c => c.Id) 
                : query.OrderByDescending(c => c.Id)
        };
        
        var totalCount = await query.CountAsync();
        
        var statuses = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (statuses, totalCount);
    }
    
    
    public async Task<int> GetCategoriesCountAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuCategories.CountAsync();
    }

}