using PizzaApp.BL.Features.Menu.Entities;

namespace PizzaApp.WebApi.Controllers.Menu.Entities;

public class MenuItemListResponse
{
    public List<MenuItemModel> MenuItems { get; set; }
}