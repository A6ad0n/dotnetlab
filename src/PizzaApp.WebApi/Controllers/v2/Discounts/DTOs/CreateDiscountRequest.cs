namespace PizzaApp.WebApi.Controllers.v2.Discounts.DTOs;

public class CreateDiscountRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public Guid StatusGuid { get; set; }
}