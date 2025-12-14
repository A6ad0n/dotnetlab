namespace PizzaApp.WebApi.Controllers.v1.Discounts.DTOs;

public class UpdateDiscountRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
}