using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PizzaApp.DataAccess.Entities;

[Table("users")]
public class UserEntity : IdentityUser<int>, IBaseEntity
{
    public Guid ExternalId { get; set; }
    public DateTime ModificationTime { get; set; }
    public DateTime CreationTime { get; set; }
    
    public int? UserInfoId { get; set; }
    public virtual UserInfoEntity? UserInfo { get; set; }
    
    public virtual ICollection<OrderEntity> Orders { get; set; }
    public virtual ICollection<UserRoleEntity> Roles { get; set; }
}