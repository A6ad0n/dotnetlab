using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using PizzaApp.DataAccess.Context;
using PizzaApp.DataAccess.Entities;
using PizzaApp.WebApi.Settings;
using Duende.IdentityServer;
using PizzaApp.WebApi.Identity;

namespace PizzaApp.WebApi.IoC;

public static class AuthorizationConfigurator
{
    public static void ConfigureServices(IServiceCollection services, PizzaAppSettings settings)
    {
        IdentityModelEventSource.ShowPII = true;

        // Identity
        services.AddIdentity<UserEntity, RoleEntity>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<PizzaAppDbContext>()
            .AddDefaultTokenProviders()
            .AddRoles<RoleEntity>();

        // IdentityServer
        services.AddIdentityServer()
            .AddInMemoryApiScopes([
                new ApiScope("api", ["role"])
            ])
            .AddInMemoryApiResources([
                new ApiResource("api", "Pizza API")
                {
                    Scopes = { "api" },
                    UserClaims = { "role" }
                }
            ])
            .AddInMemoryClients([
                new Client
                {
                    ClientId = settings.ClientId,
                    ClientSecrets = { new Secret(settings.ClientSecret.Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { "api" },
                    AllowOfflineAccess = true
                },
                new Client
                {
                    ClientId = "swagger",
                    ClientSecrets = { new Secret("swagger".Sha256()) },
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { "api" }
                }
            ])
            .AddAspNetIdentity<UserEntity>()
            .AddProfileService<ProfileService>();

        // Authentication
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            options.RequireHttpsMetadata = false;
            options.Authority = settings.IdentityServerUri;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                NameClaimType = "sub",
                RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            options.Audience = "api";
        });

        services.AddAuthorization();
    }

    public static void ConfigureApplication(IApplicationBuilder app)
    {
        app.UseIdentityServer();
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
