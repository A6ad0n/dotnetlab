using PizzaApp.BL.Features.Users.Entities;

namespace PizzaApp.WebApi.Controllers.v2.Users.DTOs;

public class UserListResponse
{
    public List<UserModel> Users { get; set; } 
}