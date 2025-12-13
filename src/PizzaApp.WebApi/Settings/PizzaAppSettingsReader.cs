namespace PizzaApp.WebApi.Settings;

public static class PizzaAppSettingsReader
{
    public static PizzaAppSettings Read(IConfiguration configuration)
    {
        return new PizzaAppSettings()
        {
            PizzaAppDbContextConnectionString = configuration.GetConnectionString("PizzaAppDbContext"),
            ServiceUri = configuration.GetValue<Uri>("Uri"),
            IdentityServerUri = configuration.GetValue<string>("IdentityServerSettings:Uri"),
            ClientId = configuration.GetValue<string>("IdentityServerSettings:ClientId"),
            ClientSecret = configuration.GetValue<string>("IdentityServerSettings:ClientSecret"),
        };
    }
}