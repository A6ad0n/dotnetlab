using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Moq;
using Npgsql;
using PizzaApp.BL.Features.Auth.Entities;
using PizzaApp.DataAccess.Context;
using PizzaApp.DataAccess.Entities;
using PizzaApp.DataAccess.Entities.Primitives;
using PizzaApp.WebApi.Controllers.v2.Authorization.DTOs;
using PizzaApp.WebApi.Tests.Helpers;
using Respawn;
using Respawn.Graph;

namespace PizzaApp.WebApi.Tests;


public class TestBase
{
    protected readonly WebApplicationFactory<Program> _testServer;

    protected static Respawner _respawner;
    
    private HttpClient? _client = null;
    protected HttpClient TestHttpClient => _client ?? _testServer.CreateClient();

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
            services.PostConfigureAll<JwtBearerOptions>(options =>
            {
                options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                    $"{settings.IdentityServerUri}/.well-known/openid-configuration",
                    new OpenIdConnectConfigurationRetriever(),
                    new HttpDocumentRetriever(TestHttpClient)
                    {
                        RequireHttps = false,
                        SendAdditionalHeaderData = true
                    });
            });
        });
    }
    
    public T? GetService<T>() where T : notnull  => _testServer.Services.GetRequiredService<T>();

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        var settings = TestSettingsHelper.GetSettings();
        await using var conn = new NpgsqlConnection(settings.PizzaAppDbContextConnectionString);
        await conn.OpenAsync();

        _respawner = await Respawner.CreateAsync(conn, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres,
            SchemasToInclude = ["public"],
            TablesToIgnore = [new Table("public","__EFMigrationsHistory")]
        });
        await AdditionalOneTimeSetUp();
    }
    
    protected virtual Task AdditionalOneTimeSetUp()
    {
        return Task.CompletedTask;
    }
    
    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        var settings = TestSettingsHelper.GetSettings();
        await using var conn = new NpgsqlConnection(settings.PizzaAppDbContextConnectionString);
        await conn.OpenAsync();
        await _respawner.ResetAsync(conn);
        await _testServer.DisposeAsync();
    }

    protected async Task SeedDatabaseAsync()
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

        var users = new[]
        {
            new UserEntity
            {
                Id = 999,
                ExternalId = new Guid("11000000-0000-0000-0000-034500000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.ru",
                NormalizedEmail = "ADMIN@ADMIN.RU",
                PhoneNumber = "77777777777",
                PasswordHash = "AQAAAAIAAYagAAAAEFagX+6l19G70fvBcw9DR0KRwJcEt1wyZoHIHjMdEGZqlVy6PL6w2aHTf8stGpZodw==",
                SecurityStamp = "5A9B8C7D-6E5F-4A3B-2C1D-0E9F8A7B6C5D",
                ConcurrencyStamp = "1A2B3C4D-5E6F-7A8B-9C0D-1E2F3A4B5C6D"
            }
        };

        var roles = new[]
        {
            new RoleEntity()
            {
                Id = 1,
                ExternalId = new Guid("11000000-0000-0000-0000-001230000000"),
                CreationTime = DateTime.UtcNow,
                ModificationTime = DateTime.UtcNow,
                Name = "Admin",
                RoleType = Role.Admin,
                Users = new List<UserRoleEntity>
                {
                    new UserRoleEntity
                    {
                        RoleId = 1,
                        UserId = 999
                    }
                }
            }
        };


        if (!db.Statuses.Any())
        {
            await db.Statuses.AddRangeAsync(statuses);
        }

        if (!db.MenuCategories.Any())
        {
            await db.MenuCategories.AddRangeAsync(categories);
        }

        if (!db.MenuItems.Any())
        {
            await db.MenuItems.AddRangeAsync(menuItems);
        }

        if (!db.Discounts.Any())
        {
            await db.Discounts.AddRangeAsync(discounts);
        }

        if (!db.Users.Any())
        {
            await db.Users.AddRangeAsync(users);
        }

        if (!db.Roles.Any())
        {
            await db.Roles.AddRangeAsync(roles);
        }
        
        
        await db.SaveChangesAsync();
    }

    protected void ClearDatabase()
    {
        using var scope = _testServer.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<PizzaAppDbContext>();
        
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }
    
    protected async Task<string> GetAdminAccessTokenAsync()
    {
        var response = await TestHttpClient.PostAsJsonAsync(
            PizzaAppEndpoints.v2.LoginUser,
            new AuthorizeUserRequest
            {
                Email = "admin@admin.ru",
                Password = "1203"
            });

        response.EnsureSuccessStatusCode();

        var tokens = await response.Content
            .ReadFromJsonAsync<TokensResponse>();

        return tokens!.AccessToken!;
    }
}