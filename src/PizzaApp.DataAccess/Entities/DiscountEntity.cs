using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaApp.DataAccess.Entities;

[Table("discounts")]
public class DiscountEntity : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    
    public int StatusId { get; set; }
    public virtual StatusEntity Status { get; set; }
    
    public virtual ICollection<MenuItemDiscountEntity> MenuItems { get; set; }
}