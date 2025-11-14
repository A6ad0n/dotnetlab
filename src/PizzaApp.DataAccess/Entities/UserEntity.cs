using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaApp.DataAccess.Entities;

[Table("users")]
public class UserEntity : BaseEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Phone  { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public int UserInfoId { get; set; }
    public virtual UserInfoEntity UserInfo { get; set; }
    
    public virtual ICollection<OrderEntity> Orders { get; set; }
    public virtual ICollection<UserRoleEntity> Roles { get; set; }
}