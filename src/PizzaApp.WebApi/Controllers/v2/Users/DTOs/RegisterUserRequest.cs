
namespace PizzaApp.WebApi.Controllers.v2.Users.DTOs;

public class RegisterUserRequest
{
    public string UserName { get; set; }
    
    public string Email { get; set; }

    public string PhoneNumber { get; set; }
    
    public string Password { get; set; }
}