using PizzaApp.BL.Features.Discounts.Entities;

namespace PizzaApp.BL.Features.Discounts.Providers;

public interface IDiscountProvider
{
    Task<DiscountModel> GetByIdAsync(int id);
    Task<List<DiscountModel>> GetAllAsync();
}