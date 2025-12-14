namespace PizzaApp.WebApi.Controllers.v2.Users.DTOs.Responses;

public class UserResponse
{
    public Guid ExternalId { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public List<RoleResponse>? Roles { get; set; }
    
    public BlockInfoResponse? BlockInformation { get; set; }
}