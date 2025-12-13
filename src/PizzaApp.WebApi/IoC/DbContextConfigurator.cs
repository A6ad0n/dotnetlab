using PizzaApp.DataAccess.Context;
using PizzaApp.WebApi.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace PizzaApp.WebApi.IoC;

public static class DbContextConfigurator
{
    public static void ConfigureService(IServiceCollection services, PizzaAppSettings settings)
    {
        services.AddDbContextFactory<PizzaAppDbContext>(options =>
        {
            options.UseNpgsql(settings.PizzaAppDbContextConnectionString);
            options.ConfigureWarnings(warnings => 
                warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }, ServiceLifetime.Scoped);
    }

    public static void ConfigureApplication(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var contextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<PizzaAppDbContext>>();
        using var context = contextFactory.CreateDbContext();
    }
}