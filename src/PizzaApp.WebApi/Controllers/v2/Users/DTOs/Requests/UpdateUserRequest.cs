namespace PizzaApp.WebApi.Controllers.v2.Users.DTOs.Requests;

public class UpdateUserRequest
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
}