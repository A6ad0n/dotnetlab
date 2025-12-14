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
            var result = await userRepository.GetByIdWithDetailsAsync(updatedUser.Id);
            return mapper.Map<UserModel>(result);
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

    public async Task<UserModel> ChangeUserRolesAsync(int userId, List<int> roleIds)
    {
        var user = await userRepository.GetByIdWithRolesAsync(userId)
                   ?? throw new BusinessLogicException<UserResultCode>(UserResultCode.UserNotFound);
        
        try
        {
            var allRoles = await userRepository.GetAllRolesAsync();
            var allRoleIds = allRoles.Select(r => r.Id).ToHashSet();
            
            var nonExistent = roleIds.Where(r => !allRoleIds.Contains(r)).ToList();
            if (nonExistent.Count != 0)
            {
                throw new BusinessLogicException<UserResultCode>(
                    UserResultCode.RolesNotFound,
                    $"Non-existent roles: {string.Join(", ", nonExistent)}"
                );
            }
            
            var newRoleIds = roleIds.Where(r => allRoleIds.Contains(r)).ToList();
            
            await userRepository.UpdateUserRolesAsync(user, newRoleIds);

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
    
    public async Task<UserModel> UpdateUserAsync(Guid userGuid, UpdateUserModel model)
    {
        var user = await userRepository.GetByGuidAsync(userGuid) ?? 
                   throw new BusinessLogicException<UserResultCode>(UserResultCode.UserNotFound);
        return await UpdateUserAsync(user.Id, model);
    }

    public async Task<UserModel> ChangeBlockInfoUserAsync(Guid userGuid, BlockInformationModel model)
    {
        var user = await userRepository.GetByGuidWithRolesAsync(userGuid)
                   ?? throw new BusinessLogicException<UserResultCode>(UserResultCode.UserNotFound);
        return await ChangeBlockInfoUserAsync(user.Id, model);
    }
    
    public async Task<UserModel> ChangeUserRolesAsync(Guid userGuid, List<Guid> roleGuids)
    {
        var user = await userRepository.GetByGuidWithRolesAsync(userGuid)
                   ?? throw new BusinessLogicException<UserResultCode>(UserResultCode.UserNotFound);
        var roles = await userRepository.GetAllRolesAsync();
        var roleIds = roles
            .Where(r => roleGuids.Contains(r.ExternalId))
            .Select(r => r.Id)
            .ToList();

        if (roleGuids.Count != roleIds.Count)
        {
            throw new BusinessLogicException<UserResultCode>(UserResultCode.RolesNotFound);
        }
        
        return await ChangeUserRolesAsync(user.Id, roleIds);
    }
    
    public async Task<bool> DeleteUserAsync(Guid userGuid)
    {
        return await userRepository.DeleteAsync(userGuid);
    }
}