using AutoMapper;
using PizzaApp.BL.Common.Exceptions;
using PizzaApp.BL.Features.Menu.Entities;
using PizzaApp.BL.Features.Menu.Exceptions;
using PizzaApp.DataAccess.Repository.MenuItemRepository;

namespace PizzaApp.BL.Features.Menu.Providers;

public class MenuProvider(IMenuItemRepository menuItemRepository, IMapper mapper) : IMenuProvider
{
    public async Task<MenuItemModel> GetByIdAsync(int id)
    {
        var menuItems = await menuItemRepository.GetByIdWithAllDataAsync(id) ??
                   throw new BusinessLogicException<MenuResultCode>(MenuResultCode.MenuItemNotFound);
        
        return mapper.Map<MenuItemModel>(menuItems);
    }
    
    public async Task<List<MenuItemModel>> GetAllAsync()
    {
        var menuItems = await menuItemRepository.GetAllWithDetailsAsync();
        return mapper.Map<List<MenuItemModel>>(menuItems);
    }
}