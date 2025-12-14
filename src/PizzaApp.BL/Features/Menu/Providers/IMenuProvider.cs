using PizzaApp.BL.Features.Menu.Entities;

namespace PizzaApp.BL.Features.Menu.Providers;

public interface IMenuProvider
{
    Task<MenuItemModel> GetByIdAsync(int id);
    Task<MenuItemModel> GetByGuidAsync(Guid guid);
    Task<List<MenuItemModel>> GetAllAsync();
}