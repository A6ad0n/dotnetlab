using System.Text;
using AutoMapper;
using Microsoft.Extensions.Logging;
using PizzaApp.BL.Common.Exceptions;
using PizzaApp.BL.Features.Users.DTOs;
using PizzaApp.BL.Features.Users.Entities;
using PizzaApp.BL.Features.Users.Exceptions;
using PizzaApp.BL.Features.Users.Validators;
using PizzaApp.DataAccess.Repository.UserRepository;

namespace PizzaApp.BL.Features.Users.Managers;

public class UsersManager(IUserRepository userRepository, IMapper mapper, ILogger<UsersManager> logger)
    : IUsersManager
{
    public async Task<UserModel> UpdateUserAsync(int userId, UpdateUserModel model)
    {
        var validationResult = await new UpdateUserModelValidator().ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            var stringBuilder = new StringBuilder();
            foreach (var error in errors)
                stringBuilder.AppendLine(error);
            throw new BusinessLogicException<UserResultCode>(UserResultCode.UserValidationFailure, 
                stringBuilder.ToString());
        }
        
        var user = await userRepository.GetByIdAsync(userId) ?? 
                   throw new BusinessLogicException<UserResultCode>(UserResultCode.UserNotFound);
        if (!string.IsNullOrWhiteSpace(model.UserName))
        {
            user.UserName = model.UserName;
        }
        if (!string.IsNullOrWhiteSpace(model.Email))
        {
            user.Email = model.Email;
        }
        if (!string.IsNullOrWhiteSpace(model.PhoneNumber))
        {
            user.PhoneNumber = model.PhoneNumber;
        }

        try
        {
            var updatedUser = await userRepository.SaveAsync(user);
            return mapper.Map<UserModel>(updatedUser);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw new BusinessLogicException<UserResultCode>(UserResultCode.UserUpdateFailure);
        }
    }

    public async Task<UserModel> ChangeBlockInfoUserAsync(int userId, BlockInformationModel model)
    {
        var validationResult = await new BlockInformationModelValidator().ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            var stringBuilder = new StringBuilder();
            foreach (var error in errors)
                stringBuilder.AppendLine(error);
            throw new BusinessLogicException<UserResultCode>(UserResultCode.UserValidationFailure,
                stringBuilder.ToString());
        }
        
        var user = await userRepository.GetByIdWithDetailsAsync(userId) ??
                   throw new BusinessLogicException<UserResultCode>(UserResultCode.UserNotFound);
        
        try
        {
            await userRepository.UpdateUserInfoAsync(user, model.IsBlocked, model.BlockInformation);
            
            var updatedUser = await userRepository.GetByIdWithDetailsAsync(userId);
            return mapper.Map<UserModel>(updatedUser);
        }
        catch (Exception e)
        {
            logger.LogError(e.Message);
            throw new BusinessLogicException<UserResultCode>(UserResultCode.UserUpdateFailure, 
                "Failed to update user block information");
        }
    }

    public async Task<UserModel> ChangeUserRolesAsync(int userId, UpdateUserRolesModel model)
    {
        var user = await userRepository.GetByIdWithRolesAsync(userId)
                   ?? throw new BusinessLogicException<UserResultCode>(UserResultCode.UserNotFound);

        try
        {
            var allRoles = await userRepository.GetAllRolesAsync();
            var allRoleIds = allRoles.Select(r => r.Id).ToHashSet();
            
            var nonExistent = model.RoleIds.Where(r => !allRoleIds.Contains(r)).ToList();
            if (nonExistent.Count != 0)
            {
                throw new BusinessLogicException<UserResultCode>(
                    UserResultCode.RolesNotFound,
                    $"Non-existent roles: {string.Join(", ", nonExistent)}"
                );
            }
            
            var newRoleIds = model.RoleIds.Where(r => allRoleIds.Contains(r)).ToList();
            
            await userRepository.UpdateUserRolesAsync(userId, newRoleIds);

            var updatedUser = await userRepository.GetByIdWithDetailsAsync(userId);
            return mapper.Map<UserModel>(updatedUser);
        }
        catch (Exception ex) when (ex is not BusinessLogicException<UserResultCode>)
        {
            logger.LogError(ex.Message);
            throw new BusinessLogicException<UserResultCode>(UserResultCode.UserUpdateFailure, "Failed to update user's roles");
        }
    }
    
    public async Task<bool> DeleteUserAsync(int userId)
    {
        return await userRepository.DeleteAsync(userId);
    }
}