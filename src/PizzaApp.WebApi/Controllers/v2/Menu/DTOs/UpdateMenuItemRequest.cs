namespace PizzaApp.WebApi.Controllers.v2.Menu.DTOs;

public class UpdateMenuItemRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal? Price { get; set; }
}