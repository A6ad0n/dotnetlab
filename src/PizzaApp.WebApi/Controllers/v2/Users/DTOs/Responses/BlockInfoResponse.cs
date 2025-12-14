namespace PizzaApp.WebApi.Controllers.v2.Users.DTOs.Responses;

public class BlockInfoResponse
{
    public bool IsBlocked { get; set; }
    public string? BlockInformation { get; set; }
}