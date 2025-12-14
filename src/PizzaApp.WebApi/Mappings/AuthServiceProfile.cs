using AutoMapper;
using PizzaApp.BL.Features.Auth.Entities;
using v1 = PizzaApp.WebApi.Controllers.v1.Authorization.DTOs;

namespace PizzaApp.WebApi.Mappings;

public class AuthServiceProfile : Profile
{
    public AuthServiceProfile()
    {
        CreateMap<v1.AuthorizeUserRequest, AuthorizeUserModel>();
    }
}