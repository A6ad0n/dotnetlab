
namespace PizzaApp.WebApi.Controllers.Authorization.DTOs;

public class AuthorizeUserRequest
{
    public string Email { get; set; }
    
    public string Password { get; set; }
    
}