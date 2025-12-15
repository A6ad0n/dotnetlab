using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace PizzaApp.WebApi.IoC
{
    public static class SwaggerConfigurator
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(2, 0);
                options.AssumeDefaultVersionWhenUnspecified = false;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader()
                );
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Введите JWT токен в формате: Bearer {токен}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PizzaApp API v1",
                    Version = "v1",
                    Description = "API with integer ID"
                });
                
                options.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "PizzaApp API v2",
                    Version = "v2",
                    Description = "API with GUID ID"
                });
                
                options.CustomSchemaIds(type => type.FullName);
                
                options.EnableAnnotations();
            });
        }

        public static void ConfigureApplication(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "PizzaApp API v1");
                options.SwaggerEndpoint("/swagger/v2/swagger.json", "PizzaApp API v2");
                
                options.DisplayRequestDuration();
            });
        }
    }
}