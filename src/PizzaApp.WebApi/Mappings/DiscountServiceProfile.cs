using AutoMapper;
using PizzaApp.BL.Features.Discounts.DTOs;
using PizzaApp.BL.Features.Discounts.Entities;
using PizzaApp.WebApi.Controllers.v2.Discounts.DTOs.Requests;
using PizzaApp.WebApi.Controllers.v2.Discounts.DTOs.Responses;
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
        
        
        CreateMap<StatusModel, v2.Responses.StatusResponse>()
            .ForMember(dest => dest.Id, opt => 
                opt.MapFrom(src => src.ExternalId));
        CreateMap<DiscountModel, v2.Responses.DiscountResponse>()
            .ForMember(dest => dest.Id, opt => 
                opt.MapFrom(src => src.ExternalId));
        CreateMap<List<DiscountModel>, v2.Responses.DiscountListResponse>()
            .ForMember(d => d.Discounts, opt => opt.MapFrom(src => src));
        
        CreateMap<UpdateDiscountRequest, UpdateDiscountModel>();
        CreateMap<CreateDiscountRequest, CreateDiscountModel>()
            .ForMember(d => d.StatusExternalId, opt => 
                opt.MapFrom(src => src.StatusGuid));

    }
}