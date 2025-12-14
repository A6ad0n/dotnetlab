namespace PizzaApp.WebApi.Controllers.v2.Menu.DTOs.Responses;

public class MenuItemResponse
{
    public Guid ExternalId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    
    public CategoryResponse Category { get; set; }
    public StatusResponse Status { get; set; }
    
    public List<DiscountResponse> Discounts { get; set; }
}