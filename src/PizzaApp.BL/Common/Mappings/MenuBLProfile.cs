using AutoMapper;
using PizzaApp.BL.Common.Primitives;
using PizzaApp.BL.Features.Menu.DTOs;
using PizzaApp.BL.Features.Menu.Entities;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.BL.Common.Mappings;

public class MenuBLProfile : Profile
{
    public MenuBLProfile()
    {
        CreateMap<StatusEntity, StatusModel>()
            .ForMember(d => d.Id,       opt => 
                opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name,     opt => 
                opt.MapFrom(s => s.Name.ToString()))
            .ForMember(d => d.StatusType, opt => 
                opt.MapFrom(s => (StatusTypeModel)(int)s.Name));
        
        CreateMap<MenuCategoryEntity, CategoryModel>()
            .ForMember(d => d.Id,       opt => 
                opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name,     opt => 
                opt.MapFrom(s => s.Name.ToString()))
            .ForMember(d => d.CategoryType, opt => 
                opt.MapFrom(s => (CategoryTypeModel)(int)s.Name));

        
        CreateMap<MenuItemEntity, MenuItemModel>()
            .ForMember(d => d.Category,
                opt => 
                opt.MapFrom(s => s.Category))
            .ForMember(d => d.Status, opt => 
                opt.MapFrom(s => s.Status))
            .ForMember(d => d.Discounts, opt => 
                opt.MapFrom(s => s.Discounts));

        CreateMap<MenuItemDiscountEntity, DiscountModel>()
            .ForMember(d => d.Name, opt => 
                opt.MapFrom(s => s.Discount.Name))
            .ForMember(d => d.Description, opt => 
                opt.MapFrom(s => s.Discount.Description))
            .ForMember(d => d.DiscountPercentage, opt => 
                opt.MapFrom(s => s.Discount.DiscountPercentage))
            .ForMember(d => d.ValidFrom, opt => 
                opt.MapFrom(s => s.Discount.ValidFrom))
            .ForMember(d => d.ValidTo, opt => 
                opt.MapFrom(s => s.Discount.ValidTo))
            .ForMember(d => d.Status, opt => 
                opt.MapFrom(s => s.Discount.Status));
        
        CreateMap<CreateMenuItemModel, MenuItemEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Discounts, opt => opt.Ignore())
            .ForMember(dest => dest.Category, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore());  
    }
}