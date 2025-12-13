using PizzaApp.BL.Features.Statuses.Entities;

namespace PizzaApp.BL.Features.Statuses.Providers;

public interface IStatusProvider
{
    Task<StatusModel> GetByIdAsync(int id);
    Task<List<StatusModel>> GetAllAsync();
}