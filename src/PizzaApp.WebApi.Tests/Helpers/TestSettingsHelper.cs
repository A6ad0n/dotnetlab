using PizzaApp.WebApi.Settings;

namespace PizzaApp.WebApi.Tests.Helpers;

public static class TestSettingsHelper
{
    public static PizzaAppSettings GetSettings()
    {
        return PizzaAppSettingsReader.Read(ConfigurationHelper.GetConfiguration());
    }
}