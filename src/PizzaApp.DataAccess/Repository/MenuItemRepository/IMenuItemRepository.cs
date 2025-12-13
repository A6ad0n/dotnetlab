using PizzaApp.DataAccess.Entities;
using PizzaApp.DataAccess.Entities.Primitives;

namespace PizzaApp.DataAccess.Repository.MenuItemRepository;

public interface IMenuItemRepository : IRepository<MenuItemEntity>
{
    Task<MenuItemEntity?> GetByIdWithDetailsAsync(int id);
    
    Task<MenuItemEntity?> GetByIdWithStatusAsync(int id);
    Task<MenuItemEntity?> GetByIdWithDiscountsAsync(int id);
    Task<MenuItemEntity?> GetByIdWithOrdersAsync(int id);
    Task<MenuItemEntity?> GetByIdWithCategoryAsync(int id);
    Task<MenuItemEntity?> GetByIdWithAllDataAsync(int id);
    
    Task<List<MenuItemEntity>> GetAllWithDetailsAsync();
    
    Task<List<MenuItemEntity>> GetMenuItemsByCategoryAsync(MenuCategory category);
    Task<List<MenuItemEntity>> GetMenuItemsByCategoryIdAsync(int categoryId);    
    Task<List<MenuItemEntity>> GetMenuItemsByStatusAsync(Status status);
    Task<List<MenuItemEntity>> GetMenuItemsByStatusIdAsync(int statusId);
    
    Task<(List<MenuItemEntity> MenuItems, int TotalCount)> GetMenuItemsPagedAsync(
        int pageNumber, 
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        bool ascending = true);
    
    Task<int> GetMenuItemsCountAsync();

    Task<bool> ExistsCategoryAsync(int categoryId);
    Task<List<MenuCategoryEntity>> GetAllCategoriesAsync();
    Task UpdateMenuItemCategoryAsync(MenuItemEntity menuItem, int categoryId);
    
    Task<bool> ExistsStatusAsync(int statusId);
    Task<List<StatusEntity>> GetAllStatusesAsync();
    Task UpdateMenuItemStatusAsync(MenuItemEntity menuItem, int statusId);
    
    Task<List<DiscountEntity>> GetAllDiscountsAsync();
    Task UpdateMenuItemDiscountsAsync(MenuItemEntity menuItem, List<int> discountIds);
    
    Task<MenuItemEntity> SaveWithDiscountsAsync(MenuItemEntity menuItem, List<int> discountIds);
}