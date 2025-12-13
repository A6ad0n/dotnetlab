using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PizzaApp.DataAccess.Entities;

[Table("user_roles")]
public class UserRoleEntity : IdentityUserRole<int>, IBaseEntity
{
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    public DateTime ModificationTime { get; set; }
    public DateTime CreationTime { get; set; }
    public virtual UserEntity User { get; set; }
    public virtual RoleEntity Role { get; set; }
}