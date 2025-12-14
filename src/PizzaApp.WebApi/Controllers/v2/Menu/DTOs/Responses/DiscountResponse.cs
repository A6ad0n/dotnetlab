namespace PizzaApp.WebApi.Controllers.v2.Menu.DTOs.Responses;

public class DiscountResponse
{
    public Guid ExternalId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidTo { get; set; }
    public StatusResponse Status { get; set; }
}