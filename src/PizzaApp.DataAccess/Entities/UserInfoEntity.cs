using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaApp.DataAccess.Entities;

[Table("user_infos")]
public class UserInfoEntity : BaseEntity
{
    public bool IsBlocked {get; set;}
    public string BlockInformation {get; set;}
    
    public virtual UserEntity User { get; set; }
}