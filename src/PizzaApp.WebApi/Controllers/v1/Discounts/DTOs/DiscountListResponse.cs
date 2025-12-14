using PizzaApp.BL.Features.Discounts.Entities;

namespace PizzaApp.WebApi.Controllers.v1.Discounts.DTOs;

public class DiscountListResponse
{
    public List<DiscountModel> Discounts { get; set; }
}