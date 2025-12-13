using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using PizzaApp.DataAccess.Context;
using PizzaApp.DataAccess.Entities;
using PizzaApp.DataAccess.Entities.Primitives;
using PizzaApp.WebApi.Tests.Helpers;

namespace PizzaApp.WebApi.Tests;


public class TestBase
{
    protected readonly WebApplicationFactory<Program> _testServer;
    protected HttpClient TestHttpClient => _testServer.CreateClient();

    public TestBase()
    {
        var settings = TestSettingsHelper.GetSettings();

        _testServer = new TestWebApplicationFactory(services =>
        {
            services.Replace(ServiceDescriptor.Scoped(_ =>
            {
                var httpClientFactoryMock = new Mock<IHttpClientFactory>();
                httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                    .Returns(TestHttpClient);
                return httpClientFactoryMock.Object;
            }));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestAuthHandler.Scheme;
                options.DefaultChallengeScheme = TestAuthHandler.Scheme;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                "Test", _ => { });

        });
    }
    
    public T? GetService<T>() where T : notnull  => _testServer.Services.GetRequiredService<T>();
    
    [OneTimeTearDown]
    public void OneTimeTearDown() => _testServer.Dispose();
    
    protected void SeedDatabase()
    {
        using var scope = _testServer.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PizzaAppDbContext>();
        
         var statuses = new[]
        {
            new StatusEntity()
            {
                Id = 1,
                ExternalId = new Guid("10000000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = Status.DiscountActive
            },
            new StatusEntity()
            {
                Id = 2,
                ExternalId = new Guid("20000000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = Status.DiscountExpired
            },
            new StatusEntity()
            {
                Id = 3,
                ExternalId = new Guid("30000000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = Status.MenuActive
            }
        };

        var categories = new[]
        {
            new MenuCategoryEntity()
            {
                Id = 1,
                ExternalId = new Guid("11000000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = MenuCategory.Drink
            },
            new MenuCategoryEntity()
            {
                Id = 2,
                ExternalId = new Guid("22000000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = MenuCategory.Pizza
            }
        };

        var menuItems = new[]
        {
            new MenuItemEntity()
            {
                Id = 10,
                ExternalId = new Guid("11100000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = "Pizza",
                Description = "Pizza with smth delicious",
                ImageUrl = "smthlikeurl",
                Price = 100,
                CategoryId = 2,
                StatusId = 3
            },
            new MenuItemEntity()
            {
                Id = 20,
                ExternalId = new Guid("22200000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = "Drink",
                Description = "Drink with smth delicious",
                ImageUrl = "smthlikeurl",
                Price = 20,
                CategoryId = 1,
                StatusId = 3
            }
        };

        var discounts = new[]
        {
            new DiscountEntity()
            {
                Id = 1,
                ExternalId = new Guid("11110000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = "Mega Discount",
                Description = "Mega Discount ESKETIT",
                DiscountPercentage = 90.0m,
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow.AddDays(10),
                StatusId = 1,
                MenuItems = new List<MenuItemDiscountEntity>
                {
                    new MenuItemDiscountEntity { MenuItemId = 10, DiscountId = 1 }
                }
            },
            new DiscountEntity()
            {
                Id = 2,
                ExternalId = new Guid("22220000-0000-0000-0000-000000000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = "Mega Summer Discount",
                Description = "Mega Discount ESKETIT",
                DiscountPercentage = 15.0m,
                ValidFrom = DateTime.UtcNow,
                ValidTo = DateTime.UtcNow.AddDays(20),
                StatusId = 2,
                MenuItems = new List<MenuItemDiscountEntity>()
            }
        };


        if (!db.Statuses.Any())
        {
            db.Statuses.AddRange(statuses);
        }

        if (!db.MenuCategories.Any())
        {
            db.MenuCategories.AddRange(categories);
        }

        if (!db.MenuItems.Any())
        {
            db.MenuItems.AddRange(menuItems);
        }

        if (!db.Discounts.Any())
        {
            db.Discounts.AddRange(discounts);
        }
        
        db.SaveChanges();
    }

    protected void ClearDatabase()
    {
        using var scope = _testServer.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PizzaAppDbContext>();
        
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }
}