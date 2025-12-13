using PizzaApp.BL.Features.Users.DTOs;
using PizzaApp.BL.Features.Users.Entities;

namespace PizzaApp.BL.Features.Users.Managers;

public interface IUsersManager
{
    Task<UserModel> UpdateUserAsync(int userId, UpdateUserModel model);
    Task<UserModel> ChangeBlockInfoUserAsync(int userId, BlockInformationModel model);
    Task<UserModel> ChangeUserRolesAsync(int userId, UpdateUserRolesModel model);
    Task<bool> DeleteUserAsync(int userId);
}