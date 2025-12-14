using PizzaApp.BL.Features.Discounts.Entities;

namespace PizzaApp.WebApi.Controllers.Discounts.Entities;

public class DiscountListResponse
{
    public List<DiscountModel> Discounts { get; set; }
}