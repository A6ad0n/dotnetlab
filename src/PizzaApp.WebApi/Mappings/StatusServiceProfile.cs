using AutoMapper;
using PizzaApp.BL.Features.Statuses.Entities;
using PizzaApp.WebApi.Controllers.v2.Statuses.DTOs.Responses;
using v1 = PizzaApp.WebApi.Controllers.v1.Statuses.DTOs;
using v2 = PizzaApp.WebApi.Controllers.v2.Statuses.DTOs;

namespace PizzaApp.WebApi.Mappings;

public class StatusServiceProfile : Profile
{
    public StatusServiceProfile()
    {
        CreateMap<List<StatusModel>, v1.StatusListResponse>()
            .ForMember(d => d.Statuses, opt => 
                opt.MapFrom(src => src));
        
        CreateMap<List<StatusModel>, StatusListResponse>()
            .ForMember(d => d.Statuses, opt => 
                opt.MapFrom(src => src));
    }
}