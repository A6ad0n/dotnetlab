using AutoMapper;
using PizzaApp.BL.Features.Auth.Entities;
using PizzaApp.BL.Features.Users.DTOs;
using PizzaApp.BL.Features.Users.Entities;
using PizzaApp.WebApi.Controllers.Authorization.DTOs;
using PizzaApp.WebApi.Controllers.Users.DTOs;
using PizzaApp.WebApi.Controllers.Users.Entities;

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
    }
}