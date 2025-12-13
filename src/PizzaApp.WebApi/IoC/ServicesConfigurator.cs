using AutoMapper;
using PizzaApp.BL.Features.Auth.Managers;
using PizzaApp.BL.Features.Auth.Providers;
using PizzaApp.BL.Features.Categories.Providers;
using PizzaApp.BL.Features.Discounts.Managers;
using PizzaApp.BL.Features.Discounts.Providers;
using PizzaApp.BL.Features.Menu.Managers;
using PizzaApp.BL.Features.Menu.Providers;
using PizzaApp.BL.Features.Statuses.Providers;
using PizzaApp.BL.Features.Users.Managers;
using PizzaApp.BL.Features.Users.Providers;
using PizzaApp.DataAccess.Repository;
using PizzaApp.DataAccess.Repository.CategoryRepository;
using PizzaApp.DataAccess.Repository.DiscountRepository;
using PizzaApp.DataAccess.Repository.MenuItemRepository;
using PizzaApp.DataAccess.Repository.StatusRepository;
using PizzaApp.DataAccess.Repository.UserRepository;
using PizzaApp.WebApi.Settings;

namespace PizzaApp.WebApi.IoC;

public static class ServicesConfigurator
{
    public static void ConfigureServices(IServiceCollection services, PizzaAppSettings settings)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMenuItemRepository, MenuItemRepository>();
        services.AddScoped<IDiscountRepository, DiscountRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();
        
        services.AddScoped<IUsersProvider, UsersProvider>();
        services.AddScoped<IUsersManager, UsersManager>();
        
        services.AddScoped<IMenuProvider, MenuProvider>();
        services.AddScoped<IMenuManager, MenuManager>();
        
        services.AddScoped<IDiscountProvider, DiscountProvider>();
        services.AddScoped<IDiscountManager, DiscountManager>();
        
        services.AddScoped<ICategoryProvider, CategoryProvider>();
        
        services.AddScoped<IStatusProvider, StatusProvider>();
        
        services.AddScoped<IAuthProvider>(x =>
            new AuthProvider(
                x.GetRequiredService<IMapper>(),
                x.GetRequiredService<IHttpClientFactory>(),
                settings.IdentityServerUri,
                settings.ClientId,
                settings.ClientSecret));
        services.AddScoped<IAuthManager, AuthManager>();
    }
}