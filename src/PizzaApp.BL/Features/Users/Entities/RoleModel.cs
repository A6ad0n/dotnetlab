using PizzaApp.BL.Common.Primitives;

namespace PizzaApp.BL.Features.Users.Entities;

public class RoleModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public RoleTypeModel RoleType { get; set; }
}