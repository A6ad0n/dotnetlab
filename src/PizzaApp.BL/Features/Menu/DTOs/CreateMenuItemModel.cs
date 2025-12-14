namespace PizzaApp.BL.Features.Menu.DTOs;

public class CreateMenuItemModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }

    public int? CategoryId { get; set; }
    public Guid? CategoryExternalId { get; set; }
    public int? StatusId { get; set; }
    public Guid? StatusExternalId { get; set; }

    public List<int>? DiscountIds { get; set; }
    public List<Guid>? DiscountExternalIds { get; set; }
}