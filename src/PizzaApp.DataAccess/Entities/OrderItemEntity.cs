using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaApp.DataAccess.Entities;

[Table("order_items")]
public class OrderItemEntity : BaseEntity
{
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountApplied { get; set; }
    
    public int OrderId { get; set; }
    public virtual OrderEntity Order { get; set; }
    
    public int MenuItemId { get; set; }
    public virtual MenuItemEntity MenuItem { get; set; }
}