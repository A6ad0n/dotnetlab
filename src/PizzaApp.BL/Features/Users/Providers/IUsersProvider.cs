using PizzaApp.BL.Features.Users.Entities;

namespace PizzaApp.BL.Features.Users.Providers;

public interface IUsersProvider
{
    Task<UserModel> GetByIdAsync(int id);
    Task<List<UserModel>> GetAllAsync();
}