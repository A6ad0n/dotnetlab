using AutoMapper;
using PizzaApp.BL.Features.Auth.Entities;
using PizzaApp.BL.Features.Users.DTOs;
using PizzaApp.BL.Features.Users.Entities;
using v1 = PizzaApp.WebApi.Controllers.v1.Users.DTOs;

namespace PizzaApp.WebApi.Mappings;

public class UserServiceProfile : Profile
{
    public UserServiceProfile()
    {
        CreateMap<v1.RegisterUserRequest, RegisterUserModel>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());
        CreateMap<v1.UpdateUserRequest, UpdateUserModel>();
        CreateMap<v1.ChangeUserBlockInfoRequest, BlockInformationModel>();
        CreateMap<List<UserModel>, v1.UserListResponse>()
            .ForMember(d => d.Users, opt => 
                opt.MapFrom(src => src));
    }
}