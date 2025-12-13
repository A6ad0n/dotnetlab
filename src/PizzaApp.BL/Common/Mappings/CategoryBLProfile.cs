using AutoMapper;
using PizzaApp.BL.Common.Primitives;
using PizzaApp.BL.Features.Categories.Entities;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.BL.Common.Mappings;

public class CategoryBLProfile : Profile
{
    public CategoryBLProfile()
    {
        CreateMap<MenuCategoryEntity, CategoryModel>()
            .ForMember(d => d.Id,       opt => 
                opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name,     opt => 
                opt.MapFrom(s => s.Name.ToString()))
            .ForMember(d => d.CategoryType, opt => 
                opt.MapFrom(s => (CategoryTypeModel)(int)s.Name));
    }
}