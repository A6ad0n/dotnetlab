namespace PizzaApp.WebApi.Controllers.v2.Menu.DTOs;

public class CreateMenuItemRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }

    public Guid CategoryGuid { get; set; }
    public Guid StatusGuid { get; set; }

    public List<Guid>? DiscountGuids { get; set; }
}