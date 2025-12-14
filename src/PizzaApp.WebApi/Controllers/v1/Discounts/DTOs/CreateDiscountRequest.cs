namespace PizzaApp.WebApi.Controllers.Discounts.Entities;

public class CreateDiscountRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public int StatusId { get; set; }
}