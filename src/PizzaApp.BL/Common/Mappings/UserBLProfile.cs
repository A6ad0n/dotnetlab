using AutoMapper;
using PizzaApp.BL.Common.Primitives;
using PizzaApp.BL.Features.Users.Entities;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.BL.Common.Mappings;


public class UserBLProfile : Profile
{
    public UserBLProfile()
    {
        CreateMap<RoleEntity, RoleModel>()
            .ForMember(d => d.Id,       opt => 
                opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name,     opt => 
                opt.MapFrom(s => s.Name))
            .ForMember(d => d.RoleType, opt => 
                opt.MapFrom(s => (RoleTypeModel)(int)s.RoleType));

        CreateMap<UserEntity, UserModel>()
            .ForMember(dest => dest.Roles,
                opt => opt.MapFrom(src =>
                    src.Roles.Select(r => r.Role).ToList()
                ))
            .ForMember(dest => dest.BlockInformation,
                opt => opt.MapFrom(src =>
                    src.UserInfo == null
                        ? null
                        : new BlockInformationModel
                        {
                            IsBlocked = src.UserInfo.IsBlocked,
                            BlockInformation = src.UserInfo.BlockInformation
                        }
                ));
    }
}