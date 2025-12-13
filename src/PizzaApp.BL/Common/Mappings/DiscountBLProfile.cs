using AutoMapper;
using PizzaApp.BL.Common.Primitives;
using PizzaApp.BL.Features.Discounts.DTOs;
using PizzaApp.BL.Features.Discounts.Entities;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.BL.Common.Mappings;

public class DiscountBLProfile : Profile
{
    public DiscountBLProfile()
    {
        CreateMap<StatusEntity, StatusModel>()
            .ForMember(d => d.Id,       opt => 
                opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name,     opt => 
                opt.MapFrom(s => s.Name.ToString()))
            .ForMember(d => d.StatusType, opt => 
                opt.MapFrom(s => (StatusTypeModel)(int)s.Name));
        
        CreateMap<DiscountEntity, DiscountModel>()
            .ForMember(d => d.Status, opt =>
                opt.MapFrom(s => s.Status));
        
        CreateMap<CreateDiscountModel, DiscountEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore());  

    }
}