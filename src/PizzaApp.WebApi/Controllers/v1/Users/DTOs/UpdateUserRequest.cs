namespace PizzaApp.WebApi.Controllers.v1.Users.DTOs;

public class UpdateUserRequest
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}