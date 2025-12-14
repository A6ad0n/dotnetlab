using PizzaApp.BL.Features.Users.Entities;

namespace PizzaApp.WebApi.Controllers.Users.Entities;

public class UserListResponse
{
    public List<UserModel> Users { get; set; } 
}