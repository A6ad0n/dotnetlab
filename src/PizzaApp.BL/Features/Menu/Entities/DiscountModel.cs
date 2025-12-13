namespace PizzaApp.BL.Features.Menu.Entities;

public class DiscountModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public StatusModel Status { get; set; }
}