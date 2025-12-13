namespace PizzaApp.BL.Features.Auth.Entities;

public class TokensResponse
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
}