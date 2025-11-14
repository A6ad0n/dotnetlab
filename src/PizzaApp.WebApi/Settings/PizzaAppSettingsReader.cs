namespace PizzaApp.WebApi.Settings;

public static class PizzaAppSettingsReader
{
    public static PizzaAppSettings Read(IConfiguration configuration)
    {
        return new PizzaAppSettings()
        {
            PizzaAppDbContextConnectionString = configuration.GetConnectionString("PizzaAppDbContext"),
        };
    }
}