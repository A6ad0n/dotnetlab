namespace PizzaApp.BL.Features.Menu.Entities;

public class MenuItemModel
{
    public int Id { get; set; }
    public Guid ExternalId { get; set; }
    public DateTime CreationTime { get; set; }
    public DateTime ModificationTime { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    
    public CategoryModel Category { get; set; }
    public StatusModel Status { get; set; }
    
    public List<DiscountModel> Discounts { get; set; }
}