using PizzaApp.BL.Features.Menu.Entities;

namespace PizzaApp.WebApi.Controllers.v1.Menu.DTOs;

public class MenuItemListResponse
{
    public List<MenuItemModel> MenuItems { get; set; }
}