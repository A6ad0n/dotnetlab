using PizzaApp.BL.Features.Users.Entities;

namespace PizzaApp.WebApi.Controllers.Users.Entities;

public class UsersListResponse
{
    public List<UserModel> Users { get; set; } 
}