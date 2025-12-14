using AutoMapper;
using PizzaApp.BL.Features.Discounts.DTOs;
using PizzaApp.BL.Features.Discounts.Entities;
using PizzaApp.WebApi.Controllers.v1.Discounts.DTOs;

namespace PizzaApp.WebApi.Mappings;

public class DiscountServiceProfile : Profile
{
    public DiscountServiceProfile()
    {
        CreateMap<UpdateDiscountRequest, UpdateDiscountModel>();
        CreateMap<CreateDiscountRequest, CreateDiscountModel>();
        CreateMap<List<DiscountModel>, DiscountListResponse>()
            .ForMember(d => d.Discounts, opt => opt.MapFrom(src => src));
    }
}