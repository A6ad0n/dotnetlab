using System.ComponentModel.DataAnnotations.Schema;
using PizzaApp.DataAccess.Entities.Primitives;

namespace PizzaApp.DataAccess.Entities;

[Table("menu_categories")]
public class MenuCategoryEntity : BaseEntity
{
    public MenuCategory Name { get; set; }
    
    public virtual ICollection<MenuItemEntity> MenuItems { get; set; }
}