using PizzaApp.BL.Features.Discounts.DTOs;
using PizzaApp.BL.Features.Discounts.Entities;

namespace PizzaApp.BL.Features.Discounts.Managers;

public interface IDiscountManager
{
    Task<DiscountModel> UpdateDiscountAsync(int discountId, UpdateDiscountModel model);
    
    Task<DiscountModel> ChangeDiscountStatusAsync(int discountId, int statusId);
    
    Task<DiscountModel> CreateDiscountAsync(CreateDiscountModel model);
    Task<bool> DeleteDiscountAsync(int discountId);
}