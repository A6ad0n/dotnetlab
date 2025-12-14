using AutoMapper;
using PizzaApp.BL.Features.Menu.DTOs;
using PizzaApp.BL.Features.Menu.Entities;
using PizzaApp.WebApi.Controllers.v2.Menu.DTOs.Requests;
using PizzaApp.WebApi.Controllers.v2.Menu.DTOs.Responses;
using v1 = PizzaApp.WebApi.Controllers.v1.Menu.DTOs;
using v2 = PizzaApp.WebApi.Controllers.v2.Menu.DTOs;

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
        
        CreateMap<UpdateMenuItemRequest, UpdateMenuItemModel>();
        CreateMap<CreateMenuItemRequest, CreateMenuItemModel>()
            .ForMember(d => d.StatusExternalId, opt => 
                opt.MapFrom(src => src.StatusGuid))
            .ForMember(d => d.CategoryExternalId, opt => 
                opt.MapFrom(src => src.CategoryGuid))
            .ForMember(d => d.DiscountExternalIds, opt => 
                opt.MapFrom(src => src.DiscountGuids));
        CreateMap<List<MenuItemModel>, MenuItemListResponse>()
            .ForMember(d => d.MenuItems, opt => 
                opt.MapFrom(src => src));
    }
}