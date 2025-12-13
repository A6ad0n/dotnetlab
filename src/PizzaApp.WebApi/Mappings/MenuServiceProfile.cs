using AutoMapper;
using PizzaApp.BL.Features.Menu.DTOs;
using PizzaApp.WebApi.Controllers.Menu.Entities;

namespace PizzaApp.WebApi.Mappings;
 
public class MenuServiceProfile : Profile
{
    public MenuServiceProfile()
    {
        CreateMap<UpdateMenuItemRequest, UpdateMenuItemModel>();
        CreateMap<CreateMenuItemRequest, CreateMenuItemModel>();
    }
}