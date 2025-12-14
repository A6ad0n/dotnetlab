using AutoMapper;
using PizzaApp.BL.Features.Auth.Entities;
using PizzaApp.BL.Features.Users.DTOs;
using PizzaApp.BL.Features.Users.Entities;
using PizzaApp.WebApi.Controllers.v1.Authorization.DTOs;
using PizzaApp.WebApi.Controllers.v1.Users.DTOs;

namespace PizzaApp.WebApi.Mappings;

public class UsersServiceProfile : Profile
{
    public UsersServiceProfile()
    {
        CreateMap<RegisterUserRequest, RegisterUserModel>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());
        CreateMap<UpdateUserRequest, UpdateUserModel>();
        CreateMap<ChangeUserBlockInfoRequest, BlockInformationModel>();
        CreateMap<AuthorizeUserRequest, AuthorizeUserModel>();
        CreateMap<ChangeUserRolesRequest, UpdateUserRolesModel>();
        CreateMap<List<UserModel>, UserListResponse>()
            .ForMember(d => d.Users, opt => 
                opt.MapFrom(src => src));
    }
}