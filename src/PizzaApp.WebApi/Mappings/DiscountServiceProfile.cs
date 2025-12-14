using AutoMapper;
using PizzaApp.BL.Features.Discounts.DTOs;
using PizzaApp.BL.Features.Discounts.Entities;
using v1 = PizzaApp.WebApi.Controllers.v1.Discounts.DTOs;
using v2 = PizzaApp.WebApi.Controllers.v2.Discounts.DTOs;

namespace PizzaApp.WebApi.Mappings;

public class DiscountServiceProfile : Profile
{
    public DiscountServiceProfile()
    {
        CreateMap<v1.UpdateDiscountRequest, UpdateDiscountModel>();
        CreateMap<v1.CreateDiscountRequest, CreateDiscountModel>();
        CreateMap<List<DiscountModel>, v1.DiscountListResponse>()
            .ForMember(d => d.Discounts, opt => opt.MapFrom(src => src));
        
        CreateMap<v2.UpdateDiscountRequest, UpdateDiscountModel>();
        CreateMap<v2.CreateDiscountRequest, CreateDiscountModel>();
        CreateMap<List<DiscountModel>, v2.DiscountListResponse>()
            .ForMember(d => d.Discounts, opt => opt.MapFrom(src => src));

    }
}