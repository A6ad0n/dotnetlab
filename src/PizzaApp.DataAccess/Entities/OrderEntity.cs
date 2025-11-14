using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaApp.DataAccess.Entities;

[Table("orders")]
public class OrderEntity : BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? StatusChangedAt { get; set; }
    public decimal TotalPrice { get; set; }
    
    public int StatusId { get; set; }
    public virtual StatusEntity Status { get; set; }
    
    public int UserId { get; set; }
    public virtual UserEntity User { get; set; }
    
    public virtual ICollection<OrderItemEntity> OrderItems { get; set; }
}