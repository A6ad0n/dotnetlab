using Microsoft.EntityFrameworkCore;
using PizzaApp.DataAccess.Context;
using PizzaApp.DataAccess.Entities;
using PizzaApp.DataAccess.Entities.Primitives;

namespace PizzaApp.DataAccess.Repository.MenuItemRepository;

public class MenuItemRepository(IDbContextFactory<PizzaAppDbContext> contextFactory)
    : Repository<MenuItemEntity>(contextFactory), IMenuItemRepository
{
    public async Task<MenuItemEntity?> GetByIdWithDetailsAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.MenuItems
            .Include(m => m.Category)
            .Include(m => m.Status)
            .Include(m => m.Discounts)
                .ThenInclude(md => md.Discount)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
    
    public async Task<MenuItemEntity?> GetByIdWithStatusAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems
            .Include(mi => mi.Status)
            .FirstOrDefaultAsync(mi => mi.Id == id);
    }
    
    public async Task<MenuItemEntity?> GetByIdWithCategoryAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems
            .Include(mi => mi.Category)
            .FirstOrDefaultAsync(mi => mi.Id == id);
    }
    
    public async Task<MenuItemEntity?> GetByIdWithDiscountsAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems
            .Include(mi => mi.Discounts)
                .ThenInclude(ur => ur.Discount)
            .FirstOrDefaultAsync(mi => mi.Id == id);
    }
    
    public async Task<MenuItemEntity?> GetByIdWithOrdersAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems
            .Include(mi => mi.OrderItems)
                .ThenInclude(o => o.Order)
            .FirstOrDefaultAsync(mi => mi.Id == id);
    }
    
    public async Task<MenuItemEntity?> GetByIdWithAllDataAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems
            .Include(mi => mi.Status)
            .Include(mi => mi.Category)
            .Include(mi => mi.Discounts)
                .ThenInclude(ur => ur.Discount)
            .Include(mi => mi.OrderItems)
                .ThenInclude(o => o.Order)
            .FirstOrDefaultAsync(mi => mi.Id == id);
    }
    
    public async Task<List<MenuItemEntity>> GetAllWithDetailsAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.MenuItems
            .Include(mi => mi.Status)
            .Include(mi => mi.Category)
            .Include(mi => mi.Discounts)
                .ThenInclude(ur => ur.Discount)
            .ToListAsync();
    }
    
    
    public async Task<List<MenuItemEntity>> GetMenuItemsByCategoryAsync(MenuCategory category)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems
            .Include(mi => mi.Category)
            .Where(mi => mi.Category.Name == category)
            .ToListAsync();
    }
    
    public async Task<List<MenuItemEntity>> GetMenuItemsByCategoryIdAsync(int categoryId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems
            .Include(mi => mi.Category)
            .Where(mi => mi.CategoryId == categoryId)
            .ToListAsync();
    }
    
        
    public async Task<List<MenuItemEntity>> GetMenuItemsByStatusAsync(Status status)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems
            .Include(mi => mi.Status)
            .Where(mi => mi.Status.Name == status)
            .ToListAsync();
    }
    
    public async Task<List<MenuItemEntity>> GetMenuItemsByStatusIdAsync(int statusId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems
            .Include(mi => mi.Status)
            .Where(mi => mi.StatusId == statusId)
            .ToListAsync();
    }

    
    public async Task<(List<MenuItemEntity> MenuItems, int TotalCount)> GetMenuItemsPagedAsync(
        int pageNumber, 
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        bool ascending = true)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        var query = context.MenuItems.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(mi => mi.Name.Contains(searchTerm));
        }
        
        query = sortBy?.ToLower() switch
        {
            "name" => ascending ? query.OrderBy(mi => mi.Name) : query.OrderByDescending(mi => mi.Name),
            "price" => ascending ? query.OrderBy(mi => mi.Price) : query.OrderByDescending(mi => mi.Price),
            "category" => ascending ? query.OrderBy(mi => mi.CategoryId) : query.OrderByDescending(mi => mi.CategoryId),
            _ => ascending ? query.OrderBy(mi => mi.Id) : query.OrderByDescending(mi => mi.Id)
        };
        
        var totalCount = await query.CountAsync();
        
        var menuItems = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return (menuItems, totalCount);
    }
    
    
    public async Task<int> GetMenuItemsCountAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems.CountAsync();
    }


    public async Task<bool> ExistsCategoryAsync(int categoryId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.MenuItems.AnyAsync(mi => mi.CategoryId == categoryId);
    }
    public async Task<List<MenuCategoryEntity>> GetAllCategoriesAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.MenuCategories.ToListAsync();
    }

    public async Task UpdateMenuItemCategoryAsync(MenuItemEntity menuItem, int categoryId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        menuItem.CategoryId = categoryId;
        menuItem.Category = await context.MenuCategories.FindAsync(categoryId);

        context.MenuItems.Update(menuItem);
        await context.SaveChangesAsync();
    }


    public async Task<bool> ExistsStatusAsync(int statusId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.MenuItems.AnyAsync(mi => mi.StatusId == statusId);
    }
    public async Task<List<StatusEntity>> GetAllStatusesAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Statuses.ToListAsync();
    }

    public async Task UpdateMenuItemStatusAsync(MenuItemEntity menuItem, int statusId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        menuItem.StatusId = statusId;
        menuItem.Status = await context.Statuses.FindAsync(statusId);

        context.MenuItems.Update(menuItem);
        await context.SaveChangesAsync();
    }


    public async Task<List<DiscountEntity>> GetAllDiscountsAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Discounts.ToListAsync();
    }
    
    public async Task UpdateMenuItemDiscountsAsync(MenuItemEntity menuItem, List<int> newDiscountIds)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var existingDiscounts = await context.MenuItemDiscounts
            .Where(ur => ur.MenuItemId == menuItem.Id)
            .ToListAsync();
        
        if (existingDiscounts.Count != 0)
            context.MenuItemDiscounts.RemoveRange(existingDiscounts);
        
        var discountsToAdd = newDiscountIds.Select(id => new MenuItemDiscountEntity()
        {
            MenuItemId = menuItem.Id,
            DiscountId = id,
            CreationTime = DateTime.UtcNow,
            ModificationTime = DateTime.UtcNow,
            ExternalId = Guid.NewGuid()
        }).ToList();

        if (discountsToAdd.Count != 0)
            context.MenuItemDiscounts.AddRange(discountsToAdd);

        await context.SaveChangesAsync();
    }

    public async Task<MenuItemEntity> SaveWithDiscountsAsync(MenuItemEntity menuItem, List<int> discountIds)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            menuItem = await SaveAsync(menuItem);
            await UpdateMenuItemDiscountsAsync(menuItem, discountIds);
            await transaction.CommitAsync();
            return menuItem; // навигационное Discounts мб не настроено будет
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}