using AutoMapper;
using PizzaApp.BL.Features.Auth.Entities;
using PizzaApp.BL.Features.Users.DTOs;
using PizzaApp.BL.Features.Users.Entities;
using PizzaApp.WebApi.Controllers.v2.Users.DTOs.Requests;
using PizzaApp.WebApi.Controllers.v2.Users.DTOs.Responses;
using v1 = PizzaApp.WebApi.Controllers.v1.Users.DTOs;
using v2 = PizzaApp.WebApi.Controllers.v2.Users.DTOs;

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
        
        
        CreateMap<v2.Requests.RegisterUserRequest, RegisterUserModel>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore());
        CreateMap<v2.Requests.UpdateUserRequest, UpdateUserModel>();
        CreateMap<v2.Requests.ChangeUserBlockInfoRequest, BlockInformationModel>();
        
        CreateMap<BlockInformationModel, v2.Responses.BlockInfoResponse>();
        CreateMap<RoleModel, v2.Responses.RoleResponse>()
            .ForMember(dest => dest.Id, opt => 
                opt.MapFrom(src => src.ExternalId));
        CreateMap<UserModel, v2.Responses.UserResponse>()
            .ForMember(dest => dest.Id, opt => 
                opt.MapFrom(src => src.ExternalId));
        CreateMap<List<UserModel>, v2.Responses.UserListResponse>()
            .ForMember(d => d.Users, opt => 
                opt.MapFrom(src => src));
    }
}