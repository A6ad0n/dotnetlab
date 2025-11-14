using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaApp.DataAccess.Entities;

[Table("menu_item_discounts")]
public class MenuItemDiscountEntity : BaseEntity
{
    public int MenuItemId { get; set; }
    public virtual MenuItemEntity MenuItem { get; set; }
    
    public int DiscountId { get; set; }
    public virtual DiscountEntity Discount { get; set; }
}