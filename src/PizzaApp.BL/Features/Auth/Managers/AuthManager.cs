using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PizzaApp.BL.Common.Exceptions;
using PizzaApp.BL.Features.Auth.Entities;
using PizzaApp.BL.Features.Auth.Exceptions;
using PizzaApp.BL.Features.Auth.Providers;
using PizzaApp.BL.Features.Auth.Validators;
using PizzaApp.BL.Features.Users.Entities;
using PizzaApp.DataAccess.Entities;

namespace PizzaApp.BL.Features.Auth.Managers;

public class AuthManager(UserManager<UserEntity> userManager, IAuthProvider authProvider, IMapper mapper)
    : IAuthManager
{
    public async Task<UserModel> RegisterUserAsync(RegisterUserModel model)
    {
        var validationResult = await new RegisterUserModelValidator().ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            var stringBuilder = new StringBuilder();
            foreach (var error in errors)
                stringBuilder.AppendLine(error);
            throw new BusinessLogicException<AuthResultCode>(AuthResultCode.RegisterValidationFailure,
                stringBuilder.ToString());
        }
        
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            throw new BusinessLogicException<AuthResultCode>(AuthResultCode.UserAlreadyExists);
        }

        user = mapper.Map<UserEntity>(model);
        user.ExternalId = Guid.NewGuid();
        var time = DateTime.UtcNow;
        user.CreationTime = time;
        user.ModificationTime = time;
        user.UserName = model.Email;

        var createResult = await userManager.CreateAsync(user, model.Password);
        if (!createResult.Succeeded)
        {
            throw new BusinessLogicException<AuthResultCode>(AuthResultCode.UserCreationFailure,
                string.Join(Environment.NewLine, createResult.Errors.Select(e => e.Description)));
        }

        return mapper.Map<UserModel>(user);
    }

    public async Task<TokensResponse> LoginUserAsync(AuthorizeUserModel model)
    {
        var validationResult = await new AuthorizeUserModelValidator().ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => x.ErrorMessage);
            var stringBuilder = new StringBuilder();
            foreach (var error in errors)
                stringBuilder.AppendLine(error);
            throw new BusinessLogicException<AuthResultCode>(AuthResultCode.AuthorizeValidationFailure,
                stringBuilder.ToString());
        }
        
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            throw new BusinessLogicException<AuthResultCode>(AuthResultCode.UserNotFound);
        }

        var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);
        if (!passwordValid)
        {
            throw new BusinessLogicException<AuthResultCode>(AuthResultCode.EmailOrPasswordIsIncorrect);
        }

        return await authProvider.AuthorizeUserAsync(user.UserName!, model.Password);
    }
    
    public async Task<TokensResponse> RefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new BusinessLogicException<AuthResultCode>(AuthResultCode.RefreshTokenIsRequired);
        }
        return await authProvider.RefreshTokenAsync(refreshToken);
    }
}