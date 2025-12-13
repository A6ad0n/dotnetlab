using AutoMapper;
using PizzaApp.BL.Features.Discounts.DTOs;
using PizzaApp.BL.Features.Discounts.Entities;
using PizzaApp.WebApi.Controllers.Discounts.Entities;

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