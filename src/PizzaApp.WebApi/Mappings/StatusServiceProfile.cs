using AutoMapper;
using PizzaApp.BL.Features.Discounts.Entities;
using PizzaApp.WebApi.Controllers.Statuses.DTOs;

namespace PizzaApp.WebApi.Mappings;

public class StatusServiceProfile : Profile
{
    public StatusServiceProfile()
    {
        CreateMap<List<StatusModel>, StatusListResponse>()
            .ForMember(d => d.Statuses, opt => 
                opt.MapFrom(src => src));
    }
}