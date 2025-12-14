namespace PizzaApp.WebApi.Controllers.v2.Users.DTOs.Requests;

public class ChangeUserRolesRequest
{
    public List<Guid> RoleGuids { get; set; }
}
