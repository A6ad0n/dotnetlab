using PizzaApp.BL.Features.Statuses.Entities;

namespace PizzaApp.BL.Features.Statuses.Providers;

public interface IStatusProvider
{
    Task<StatusModel> GetByIdAsync(int id);
    Task<StatusModel> GetByGuidAsync(Guid guid);
    Task<List<StatusModel>> GetAllAsync();
}