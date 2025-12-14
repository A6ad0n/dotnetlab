
namespace PizzaApp.WebApi.Controllers.v1.Authorization.DTOs;

public class AuthorizeUserRequest
{
    public string Email { get; set; }
    
    public string Password { get; set; }
    
}