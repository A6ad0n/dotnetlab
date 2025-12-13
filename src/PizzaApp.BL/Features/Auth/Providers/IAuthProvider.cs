using PizzaApp.BL.Features.Auth.Entities;

namespace PizzaApp.BL.Features.Auth.Providers;

public interface IAuthProvider
{
    Task<TokensResponse> AuthorizeUserAsync(string username, string password);
    Task<TokensResponse> RefreshTokenAsync(string refreshToken);
}