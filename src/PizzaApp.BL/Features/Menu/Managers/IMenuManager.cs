using PizzaApp.BL.Features.Menu.DTOs;
using PizzaApp.BL.Features.Menu.Entities;

namespace PizzaApp.BL.Features.Menu.Managers;

public interface IMenuManager
{
    Task<MenuItemModel> UpdateMenuItemAsync(int menuItemId, UpdateMenuItemModel model);
    
    Task<MenuItemModel> ChangeMenuItemCategoryAsync(int menuItemId, int categoryId);
    Task<MenuItemModel> ChangeMenuItemStatusAsync(int menuItemId, int statusId);
    Task<MenuItemModel> ChangeMenuItemDiscountsAsync(int menuItemId, List<int> discountIds);
    
    Task<MenuItemModel> CreateMenuItemAsync(CreateMenuItemModel model);
    Task<bool> DeleteMenuItemAsync(int menuItemId);
}