using System.ComponentModel.DataAnnotations.Schema;
using PizzaApp.DataAccess.Entities.Primitives;

namespace PizzaApp.DataAccess.Entities;

[Table("roles")]
public class RoleEntity : BaseEntity
{
    public Role Name { get; set; }
    
    public virtual ICollection<UserRoleEntity> Users { get; set; }
}