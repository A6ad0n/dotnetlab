using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaApp.DataAccess.Entities;

[Table("user_roles")]
public class UserRoleEntity : BaseEntity
{
    public int UserId { get; set; }
    public virtual UserEntity User { get; set; }
    
    public int RoleId { get; set; }
    public virtual RoleEntity Role { get; set; }
}