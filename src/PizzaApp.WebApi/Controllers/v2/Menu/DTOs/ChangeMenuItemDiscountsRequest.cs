namespace PizzaApp.WebApi.Controllers.v2.Menu.DTOs;

public class ChangeMenuItemDiscountsRequest
{
    public List<Guid> DiscountGuids { get; set; }
}