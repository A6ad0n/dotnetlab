using PizzaApp.BL.Features.Auth.Entities;
using PizzaApp.BL.Features.Users.Entities;

namespace PizzaApp.BL.Features.Auth.Managers;

public interface IAuthManager
{
    Task<UserModel> RegisterUserAsync(RegisterUserModel model);
    Task<TokensResponse> LoginUserAsync(AuthorizeUserModel model);
    Task<TokensResponse> RefreshTokenAsync(string refreshToken);
}