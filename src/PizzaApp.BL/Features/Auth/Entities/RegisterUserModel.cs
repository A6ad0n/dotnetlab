using PizzaApp.DataAccess.Entities.Primitives;

namespace PizzaApp.BL.Features.Auth.Entities;

public class RegisterUserModel
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public List<Role> Roles { get; set; }
}