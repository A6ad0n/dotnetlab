using AutoMapper;
using PizzaApp.BL.Features.Menu.DTOs;
using PizzaApp.BL.Features.Menu.Entities;
using PizzaApp.WebApi.Controllers.Menu.Entities;

namespace PizzaApp.WebApi.Mappings;
 
public class MenuServiceProfile : Profile
{
    public MenuServiceProfile()
    {
        CreateMap<UpdateMenuItemRequest, UpdateMenuItemModel>();
        CreateMap<CreateMenuItemRequest, CreateMenuItemModel>();
        CreateMap<List<MenuItemModel>, MenuItemListResponse>()
            .ForMember(d => d.MenuItems, opt => 
                opt.MapFrom(src => src));
    }
}