namespace PizzaApp.WebApi.Settings;

public class PizzaAppSettings
{
    public string PizzaAppDbContextConnectionString { get; set; }

    public Uri ServiceUri { get; set; }
    public string IdentityServerUri { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}