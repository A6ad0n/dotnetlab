namespace PizzaApp.WebApi.Controllers.Users.Entities;

public class UpdateUserRequest
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}