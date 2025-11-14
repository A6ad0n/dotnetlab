using System.ComponentModel.DataAnnotations.Schema;
using PizzaApp.DataAccess.Entities.Primitives;

namespace PizzaApp.DataAccess.Entities;

[Table("statuses")]
public class StatusEntity : BaseEntity
{
    public Status Name { get; set; }
    
    public virtual ICollection<OrderEntity> Orders { get; set; }
    public virtual ICollection<MenuItemEntity> MenuItems { get; set; }
    public virtual ICollection<DiscountEntity> Discounts { get; set; }
}