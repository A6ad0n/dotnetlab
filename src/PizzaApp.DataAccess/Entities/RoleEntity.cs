using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using PizzaApp.DataAccess.Entities.Primitives;

namespace PizzaApp.DataAccess.Entities;

[Table("roles")]
public class RoleEntity : IdentityRole<int>, IBaseEntity
{
    public Guid ExternalId { get; set; }
    public DateTime ModificationTime { get; set; }
    public DateTime CreationTime { get; set; }
    public Role RoleType { get; set; }
    
    public virtual ICollection<UserRoleEntity> Users { get; set; }
}