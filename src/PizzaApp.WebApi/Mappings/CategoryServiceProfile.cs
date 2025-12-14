using AutoMapper;
using PizzaApp.BL.Features.Categories.Entities;
using v1 = PizzaApp.WebApi.Controllers.v1.Categories.DTOs;

namespace PizzaApp.WebApi.Mappings;

public class CategoryServiceProfile : Profile
{
    public CategoryServiceProfile()
    {
        CreateMap<List<CategoryModel>, v1.CategoryListResponse>()
            .ForMember(d => d.Categories, opt => 
                opt.MapFrom(src => src));
    }
}