using PizzaApp.BL.Features.Menu.Entities;

namespace PizzaApp.WebApi.Controllers.v2.Menu.DTOs.Responses;

public class MenuItemListResponse
{
    public List<MenuItemResponse> MenuItems { get; set; }
}