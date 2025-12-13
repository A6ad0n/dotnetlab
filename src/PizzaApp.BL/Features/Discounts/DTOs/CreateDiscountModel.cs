namespace PizzaApp.BL.Features.Discounts.DTOs;

public class CreateDiscountModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public int StatusId { get; set; }
}