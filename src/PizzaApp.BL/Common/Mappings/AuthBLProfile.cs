using AutoMapper;
using IdentityModel.Client;
using PizzaApp.BL.Features.Auth.Entities;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.BL.Common.Mappings;

public class AuthBLProfile : Profile
{
    public AuthBLProfile()
    {
        CreateMap<RegisterUserModel, UserEntity>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ExternalId, opt => opt.Ignore())
            .ForMember(dest => dest.CreationTime, opt => opt.Ignore())
            .ForMember(dest => dest.ModificationTime, opt => opt.Ignore())
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src =>
                src.Roles.Select(r => new RoleEntity { RoleType = r })));

        CreateMap<TokenResponse, TokensResponse>()
            .ForMember(dest => dest.AccessToken, opt => opt.MapFrom(src => src.AccessToken))
            .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.RefreshToken));

    }
}