using AutoMapper;
using PizzaApp.BL.Common.Primitives;
using PizzaApp.BL.Features.Statuses.Entities;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.BL.Common.Mappings;

public class StatusBLProfile : Profile
{
    public StatusBLProfile()
    {
        CreateMap<StatusEntity, StatusModel>()
            .ForMember(d => d.Id,       opt => 
                opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name,     opt => 
                opt.MapFrom(s => s.Name.ToString()))
            .ForMember(d => d.StatusType, opt => 
                opt.MapFrom(s => (StatusTypeModel)(int)s.Name));
    }
}