
namespace PizzaApp.WebApi.Controllers.v2.Authorization.DTOs;

public class AuthorizeUserRequest
{
    public string Email { get; set; }
    
    public string Password { get; set; }
    
}