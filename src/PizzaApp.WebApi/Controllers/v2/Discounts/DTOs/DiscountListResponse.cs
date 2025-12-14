using PizzaApp.BL.Features.Discounts.Entities;

namespace PizzaApp.WebApi.Controllers.v2.Discounts.DTOs;

public class DiscountListResponse
{
    public List<DiscountModel> Discounts { get; set; }
}