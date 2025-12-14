using AutoMapper;
using PizzaApp.BL.Features.Categories.Entities;
using PizzaApp.WebApi.Controllers.v2.Categories.DTOs.Responses;
using v1 = PizzaApp.WebApi.Controllers.v1.Categories.DTOs;
using v2 = PizzaApp.WebApi.Controllers.v2.Categories.DTOs;

namespace PizzaApp.WebApi.Mappings;

public class CategoryServiceProfile : Profile
{
    public CategoryServiceProfile()
    {
        CreateMap<List<CategoryModel>, v1.CategoryListResponse>()
            .ForMember(d => d.Categories, opt => 
                opt.MapFrom(src => src));
        
        CreateMap<CategoryModel, v2.Responses.CategoryResponse>()
            .ForMember(dest => dest.Id, opt => 
                opt.MapFrom(src => src.ExternalId));
        CreateMap<List<CategoryModel>, v2.Responses.CategoryListResponse>()
            .ForMember(d => d.Categories, opt => 
                opt.MapFrom(src => src));
    }
}