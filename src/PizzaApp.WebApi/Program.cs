using PizzaApp.WebApi.IoC;
using PizzaApp.WebApi.Middlewares;
using PizzaApp.WebApi.Settings;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json")
    .Build();

var settings = PizzaAppSettingsReader.Read(configuration);

builder.Services.AddControllers();

DbContextConfigurator.ConfigureService(builder.Services, settings);
SerilogConfigurator.ConfigureService(builder);
SwaggerConfigurator.ConfigureServices(builder.Services);
MapperConfigurator.ConfigureServices(builder.Services);
ServicesConfigurator.ConfigureServices(builder.Services, settings);
AuthorizationConfigurator.ConfigureServices(builder.Services, settings);

var app = builder.Build();

SerilogConfigurator.ConfigureApplication(app);
SwaggerConfigurator.ConfigureApplication(app);
DbContextConfigurator.ConfigureApplication(app);
AuthorizationConfigurator.ConfigureApplication(app);

app.UseHttpsRedirection();

app.UseGlobalExceptionHandling();
app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program;
