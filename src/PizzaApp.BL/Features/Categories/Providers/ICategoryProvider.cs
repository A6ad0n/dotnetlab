using PizzaApp.BL.Features.Categories.Entities;

namespace PizzaApp.BL.Features.Categories.Providers;

public interface ICategoryProvider
{
    Task<CategoryModel> GetByIdAsync(int id);
    Task<List<CategoryModel>> GetAllAsync();
}