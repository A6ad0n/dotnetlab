namespace PizzaApp.WebApi.Controllers.v2.Users.DTOs.Requests;

public class ChangeUserBlockInfoRequest
{
    public bool? IsBlocked { get; set; }
    public string? BlockInformation { get; set; }
}