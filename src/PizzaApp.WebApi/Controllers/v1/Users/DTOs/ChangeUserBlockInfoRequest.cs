namespace PizzaApp.WebApi.Controllers.Users.DTOs;

public class ChangeUserBlockInfoRequest
{
    public bool? IsBlocked { get; set; }
    public string? BlockInformation { get; set; }
}