namespace PizzaApp.WebApi.Tests;

public static class PizzaAppEndpoints
{
    public static class v2
    {
        public const string LoginUser = "api/v2/Auth/login";
        public const string RegisterUser = "api/v2/Auth/register";
        public const string RefreshToken = "api/v2/Auth/refresh";

        public const string Discounts = "api/v2/Discounts";
    }
}