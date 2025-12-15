using System.Transactions;
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
    
    public async Task<MenuItemEntity?> GetByGuidWithDetailsAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.MenuItems
            .Include(m => m.Category)
            .Include(m => m.Status)
            .Include(m => m.Discounts)
            .ThenInclude(md => md.Discount)
            .FirstOrDefaultAsync(m => m.ExternalId == guid);
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
    
    public async Task<MenuItemEntity?> GetByGuidWithStatusAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems
            .Include(mi => mi.Status)
            .FirstOrDefaultAsync(mi => mi.ExternalId == guid);
    }
    
    public async Task<MenuItemEntity?> GetByGuidWithCategoryAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems
            .Include(mi => mi.Category)
            .FirstOrDefaultAsync(mi => mi.ExternalId == guid);
    }
    
    public async Task<MenuItemEntity?> GetByGuidWithDiscountsAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems
            .Include(mi => mi.Discounts)
            .ThenInclude(ur => ur.Discount)
            .FirstOrDefaultAsync(mi => mi.ExternalId == guid);
    }
    
    public async Task<MenuItemEntity?> GetByGuidWithOrdersAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems
            .Include(mi => mi.OrderItems)
            .ThenInclude(o => o.Order)
            .FirstOrDefaultAsync(mi => mi.ExternalId == guid);
    }
    
    public async Task<MenuItemEntity?> GetByGuidWithAllDataAsync(Guid guid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems
            .Include(mi => mi.Status)
            .Include(mi => mi.Category)
            .Include(mi => mi.Discounts)
            .ThenInclude(ur => ur.Discount)
            .Include(mi => mi.OrderItems)
            .ThenInclude(o => o.Order)
            .FirstOrDefaultAsync(mi => mi.ExternalId == guid);
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
    
    public async Task<List<MenuItemEntity>> GetMenuItemsByCategoryGuidAsync(Guid categoryGuid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems
            .Include(mi => mi.Category)
            .Where(mi => mi.Category.ExternalId == categoryGuid)
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
    
    public async Task<List<MenuItemEntity>> GetMenuItemsByStatusGuidAsync(Guid statusGuid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        return await context.MenuItems
            .Include(mi => mi.Status)
            .Where(mi => mi.Status.ExternalId == statusGuid)
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
        return await context.MenuCategories.AnyAsync(mc => mc.Id == categoryId);
    }
    public async Task<bool> ExistsCategoryAsync(Guid categoryGuid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.MenuCategories.AnyAsync(mc => mc.ExternalId == categoryGuid);
    }
    public async Task<List<MenuCategoryEntity>> GetAllCategoriesAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.MenuCategories.ToListAsync();
    }

    public async Task UpdateMenuItemCategoryAsync(int menuItemId, int categoryId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var affected = await context.MenuItems
            .Where(m => m.Id == menuItemId)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(m => m.CategoryId, categoryId));

        if (affected == 0)
            throw new InvalidOperationException($"MenuItem with ID {menuItemId} not found");
    }
    
    public async Task UpdateMenuItemCategoryAsync(Guid menuItemGuid, Guid categoryGuid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var category = await context.MenuCategories
            .Where(c => c.ExternalId == categoryGuid)
            .FirstOrDefaultAsync();
        var menuItemId = await context.MenuItems
            .Where(m => m.ExternalId == menuItemGuid)
            .Select(m => m.Id)
            .FirstOrDefaultAsync();
        
        if (category == null)
            throw new InvalidOperationException($"Category with GUID {categoryGuid} not found");

        await UpdateMenuItemCategoryAsync(menuItemId, category.Id);
    }


    public async Task<bool> ExistsStatusAsync(int statusId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Statuses.AnyAsync(s => s.Id == statusId);
    }
    public async Task<bool> ExistsStatusAsync(Guid statusGuid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Statuses
            .AnyAsync(s => s.ExternalId == statusGuid);
    }
    public async Task<List<StatusEntity>> GetAllStatusesAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Statuses.ToListAsync();
    }

    public async Task UpdateMenuItemStatusAsync(int menuItemId, int statusId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var affected = await context.MenuItems
            .Where(m => m.Id == menuItemId)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(m => m.StatusId, statusId));

        if (affected == 0)
            throw new InvalidOperationException($"MenuItem with ID {menuItemId} not found");
    }
    
    public async Task UpdateMenuItemStatusAsync(Guid menuItemGuid, Guid statusGuid)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var status = await context.Statuses
            .Where(s => s.ExternalId == statusGuid)
            .FirstOrDefaultAsync();
        var menuItemId = await context.MenuItems
            .Where(m => m.ExternalId == menuItemGuid)
            .Select(m => m.Id)
            .FirstOrDefaultAsync();
        
        if (status == null)
            throw new InvalidOperationException($"Status with GUID {statusGuid} not found");

        await UpdateMenuItemStatusAsync(menuItemId, status.Id);
    }


    public async Task<List<DiscountEntity>> GetAllDiscountsAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Discounts.ToListAsync();
    }
    
    public async Task UpdateMenuItemDiscountsAsync(int menuItemId, List<int> newDiscountIds)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var menuItemExists = await context.MenuItems
            .AnyAsync(m => m.Id == menuItemId);

        if (!menuItemExists)
            throw new InvalidOperationException($"MenuItem with ID {menuItemId} not found");

        var newIds = newDiscountIds.Distinct().ToHashSet();

        var existingIds = await context.MenuItemDiscounts
            .Where(md => md.MenuItemId == menuItemId)
            .Select(md => md.DiscountId)
            .ToListAsync();

        var toRemove = existingIds.Except(newIds).ToList();
        var toAdd = newIds.Except(existingIds).ToList();

        if (toRemove.Count > 0)
        {
            await context.MenuItemDiscounts
                .Where(md => md.MenuItemId == menuItemId && toRemove.Contains(md.DiscountId))
                .ExecuteDeleteAsync();
        }

        if (toAdd.Count > 0)
        {
            var now = DateTime.UtcNow;

            context.MenuItemDiscounts.AddRange(
                toAdd.Select(discountId => new MenuItemDiscountEntity
                {
                    MenuItemId = menuItemId,
                    DiscountId = discountId,
                    ExternalId = Guid.NewGuid(),
                    CreationTime = now,
                    ModificationTime = now
                })
            );
        }

        await context.SaveChangesAsync();
    }
    
    public async Task UpdateMenuItemDiscountsAsync(Guid menuItemGuid, List<Guid> newDiscountGuids)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        var menuItem = await context.MenuItems.Where(m => m.ExternalId == menuItemGuid).FirstOrDefaultAsync();
        if (menuItem == null)
            throw new InvalidOperationException($"MenuItem with GUID {menuItemGuid} not found");
        
        var discountIds = await context.Discounts
            .Where(d => newDiscountGuids.Contains(d.ExternalId))
            .Select(d => d.Id)
            .ToListAsync();

        await UpdateMenuItemDiscountsAsync(menuItem.Id, discountIds);
    }

    public async Task<MenuItemEntity> SaveWithDiscountsAsync(MenuItemEntity menuItem, List<int> discountIds)
    {
        using var scope = new TransactionScope( TransactionScopeAsyncFlowOption.Enabled);
        menuItem = await SaveAsync(menuItem);
        await UpdateMenuItemDiscountsAsync(menuItem.Id, discountIds);
        scope.Complete();
        return menuItem;
    }
    
    public async Task<MenuItemEntity> SaveWithDiscountsAsync(MenuItemEntity menuItem, List<Guid> discountGuids)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var discountIds = await context.Discounts
            .Where(d => discountGuids.Contains(d.ExternalId))
            .Select(d => d.Id)
            .ToListAsync();

        if (discountIds.Count != discountGuids.Count)
            throw new InvalidOperationException("One or more discounts not found");

        return await SaveWithDiscountsAsync(menuItem, discountIds);
    }
}