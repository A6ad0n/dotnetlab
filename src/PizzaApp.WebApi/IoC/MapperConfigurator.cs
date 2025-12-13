using PizzaApp.BL.Common.Mappings;
using PizzaApp.WebApi.Mappings;

namespace PizzaApp.WebApi.IoC;

public static class MapperConfigurator
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(config =>
        {
            config.AddProfile<AuthBLProfile>();
            
            config.AddProfile<UsersServiceProfile>();
            config.AddProfile<UserBLProfile>();
            
            config.AddProfile<MenuBLProfile>();
            config.AddProfile<MenuServiceProfile>();
            
            config.AddProfile<DiscountBLProfile>();
            config.AddProfile<DiscountServiceProfile>();
            
            config.AddProfile<CategoryBLProfile>();
            config.AddProfile<CategoryServiceProfile>();
            
            config.AddProfile<StatusBLProfile>();
            config.AddProfile<StatusServiceProfile>();
        });
    }
}