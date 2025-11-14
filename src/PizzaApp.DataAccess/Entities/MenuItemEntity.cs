using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaApp.DataAccess.Entities;

[Table("menu_items")]
public class MenuItemEntity : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    
    public int CategoryId { get; set; }
    public virtual MenuCategoryEntity Category { get; set; }
    
    public int StatusId { get; set; }
    public virtual StatusEntity Status { get; set; }
    
    public virtual ICollection<MenuItemDiscountEntity> Discounts { get; set; }
    
    public virtual ICollection<OrderItemEntity> OrderItems { get; set; }
}