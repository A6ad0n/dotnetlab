using PizzaApp.BL.Features.Menu.DTOs;
using PizzaApp.BL.Features.Menu.Entities;

namespace PizzaApp.BL.Features.Menu.Managers;

public interface IMenuManager
{
    Task<MenuItemModel> UpdateMenuItemAsync(int menuItemId, UpdateMenuItemModel model);
    Task<MenuItemModel> UpdateMenuItemAsync(Guid menuItemGuid, UpdateMenuItemModel model);
    
    Task<MenuItemModel> ChangeMenuItemCategoryAsync(int menuItemId, int categoryId);
    Task<MenuItemModel> ChangeMenuItemStatusAsync(int menuItemId, int statusId);
    Task<MenuItemModel> ChangeMenuItemDiscountsAsync(int menuItemId, List<int> discountIds);
    
    Task<MenuItemModel> ChangeMenuItemCategoryAsync(Guid menuItemGuid, Guid categoryGuid);
    Task<MenuItemModel> ChangeMenuItemStatusAsync(Guid menuItemGuid, Guid statusGuid);
    Task<MenuItemModel> ChangeMenuItemDiscountsAsync(Guid menuItemGuid, List<Guid> discountGuids);
    
    Task<MenuItemModel> CreateMenuItemAsync(CreateMenuItemModel model);
    Task<bool> DeleteMenuItemAsync(int menuItemId);
    Task<bool> DeleteMenuItemAsync(Guid menuItemGuid);
}