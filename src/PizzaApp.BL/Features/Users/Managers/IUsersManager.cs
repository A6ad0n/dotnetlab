using PizzaApp.BL.Features.Users.DTOs;
using PizzaApp.BL.Features.Users.Entities;

namespace PizzaApp.BL.Features.Users.Managers;

public interface IUsersManager
{
    Task<UserModel> UpdateUserAsync(int userId, UpdateUserModel model);
    Task<UserModel> ChangeBlockInfoUserAsync(int userId, BlockInformationModel model);
    Task<UserModel> ChangeUserRolesAsync(int userId, List<int> roleIds);
    Task<bool> DeleteUserAsync(int userId);
    
    Task<UserModel> UpdateUserAsync(Guid userGuid, UpdateUserModel model);
    Task<UserModel> ChangeBlockInfoUserAsync(Guid userGuid, BlockInformationModel model);
    Task<UserModel> ChangeUserRolesAsync(Guid userGuid, List<Guid> roleGuids);
    Task<bool> DeleteUserAsync(Guid userGuid);
}