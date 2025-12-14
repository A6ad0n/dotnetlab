using AutoMapper;
using PizzaApp.BL.Features.Menu.DTOs;
using PizzaApp.BL.Features.Menu.Entities;
using v1 = PizzaApp.WebApi.Controllers.v1.Menu.DTOs;

namespace PizzaApp.WebApi.Mappings;
 
public class MenuServiceProfile : Profile
{
    public MenuServiceProfile()
    {
        CreateMap<v1.UpdateMenuItemRequest, UpdateMenuItemModel>();
        CreateMap<v1.CreateMenuItemRequest, CreateMenuItemModel>();
        CreateMap<List<MenuItemModel>, v1.MenuItemListResponse>()
            .ForMember(d => d.MenuItems, opt => 
                opt.MapFrom(src => src));
    }
}