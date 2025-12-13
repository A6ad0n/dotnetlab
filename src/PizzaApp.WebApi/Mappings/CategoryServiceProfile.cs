using AutoMapper;
using PizzaApp.BL.Features.Categories.Entities;
using PizzaApp.WebApi.Controllers.Categories.DTOs;

namespace PizzaApp.WebApi.Mappings;

public class CategoryServiceProfile : Profile
{
    public CategoryServiceProfile()
    {
        CreateMap<List<CategoryModel>, CategoryListResponse>()
            .ForMember(d => d.Categories, opt => 
                opt.MapFrom(src => src));
    }
}