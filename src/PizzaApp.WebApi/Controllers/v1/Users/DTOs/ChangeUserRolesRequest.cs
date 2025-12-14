namespace PizzaApp.WebApi.Controllers.v1.Users.DTOs;

public class ChangeUserRolesRequest
{
    public List<int> RoleIds { get; set; }
}
