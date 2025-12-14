namespace PizzaApp.WebApi.Controllers.v2.Menu.DTOs.Requests;

public class ChangeMenuItemDiscountsRequest
{
    public List<Guid> DiscountGuids { get; set; }
}