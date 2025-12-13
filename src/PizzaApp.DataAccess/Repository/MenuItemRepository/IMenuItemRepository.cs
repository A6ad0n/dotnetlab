using PizzaApp.DataAccess.Entities;
using PizzaApp.DataAccess.Entities.Primitives;

namespace PizzaApp.DataAccess.Repository.MenuItemRepository;

public interface IMenuItemRepository : IRepository<MenuItemEntity>
{
    Task<MenuItemEntity?> GetByIdWithDetailsAsync(int id);
    Task<MenuItemEntity?> GetByGuidWithDetailsAsync(Guid guid);
    
    Task<MenuItemEntity?> GetByIdWithStatusAsync(int id);
    Task<MenuItemEntity?> GetByIdWithDiscountsAsync(int id);
    Task<MenuItemEntity?> GetByIdWithOrdersAsync(int id);
    Task<MenuItemEntity?> GetByIdWithCategoryAsync(int id);
    Task<MenuItemEntity?> GetByIdWithAllDataAsync(int id);
    
    Task<MenuItemEntity?> GetByGuidWithStatusAsync(Guid guid);
    Task<MenuItemEntity?> GetByGuidWithDiscountsAsync(Guid guid);
    Task<MenuItemEntity?> GetByGuidWithOrdersAsync(Guid guid);
    Task<MenuItemEntity?> GetByGuidWithCategoryAsync(Guid guid);
    Task<MenuItemEntity?> GetByGuidWithAllDataAsync(Guid guid);
    
    Task<List<MenuItemEntity>> GetAllWithDetailsAsync();
    
    Task<List<MenuItemEntity>> GetMenuItemsByCategoryAsync(MenuCategory category);
    Task<List<MenuItemEntity>> GetMenuItemsByCategoryIdAsync(int categoryId);    
    Task<List<MenuItemEntity>> GetMenuItemsByCategoryGuidAsync(Guid categoryGuid);    
    Task<List<MenuItemEntity>> GetMenuItemsByStatusAsync(Status status);
    Task<List<MenuItemEntity>> GetMenuItemsByStatusIdAsync(int statusId);
    Task<List<MenuItemEntity>> GetMenuItemsByStatusGuidAsync(Guid statusGuid);
    
    Task<(List<MenuItemEntity> MenuItems, int TotalCount)> GetMenuItemsPagedAsync(
        int pageNumber, 
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        bool ascending = true);
    
    Task<int> GetMenuItemsCountAsync();

    Task<bool> ExistsCategoryAsync(int categoryId);
    Task<bool> ExistsCategoryAsync(Guid categoryGuid);
    Task<List<MenuCategoryEntity>> GetAllCategoriesAsync();
    Task UpdateMenuItemCategoryAsync(MenuItemEntity menuItem, int categoryId);
    Task UpdateMenuItemCategoryAsync(MenuItemEntity menuItem, Guid categoryGuid);
    
    Task<bool> ExistsStatusAsync(int statusId);
    Task<bool> ExistsStatusAsync(Guid statusGuid);
    Task<List<StatusEntity>> GetAllStatusesAsync();
    Task UpdateMenuItemStatusAsync(MenuItemEntity menuItem, int statusId);
    Task UpdateMenuItemStatusAsync(MenuItemEntity menuItem, Guid statusGuid);
    
    Task<List<DiscountEntity>> GetAllDiscountsAsync();
    Task UpdateMenuItemDiscountsAsync(MenuItemEntity menuItem, List<int> discountIds);
    Task UpdateMenuItemDiscountsAsync(MenuItemEntity menuItem, List<Guid> discountGuids);
    
    Task<MenuItemEntity> SaveWithDiscountsAsync(MenuItemEntity menuItem, List<int> discountIds);
    Task<MenuItemEntity> SaveWithDiscountsAsync(MenuItemEntity menuItem, List<Guid> discountGuids);
}