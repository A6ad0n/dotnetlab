using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PizzaApp.BL.Features.Discounts.Entities;
using PizzaApp.DataAccess.Context;
using PizzaApp.DataAccess.Entities;
using PizzaApp.DataAccess.Entities.Primitives;
using PizzaApp.WebApi.Controllers.Discounts.Entities;
using PizzaApp.WebApi.Tests;

namespace PizzaApp.WebApi.Tests.Discounts;

public class DiscountsControllerTests : TestBase
{
    private void AddJwtToken(HttpClient client, string role = "Admin")
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "TestUser"),
            new Claim(ClaimTypes.Role, role)
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("12345678901234567890123456789012"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "test",
            audience: "test",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );
        
        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
    }
    [Test]
    public async Task GetDiscounts_Success()
    {
        ClearDatabase();
        SeedDatabase();
        
        var response = await TestHttpClient.GetAsync("/Discounts");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadFromJsonAsync<DiscountListResponse>();
        Assert.That(content, Is.Not.Null);
        Assert.That(content.Discounts.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task GetDiscounts_NotFound()
    {
        ClearDatabase();
        
        var response = await TestHttpClient.GetAsync("/Discounts");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetDiscountById_Success()
    {
        ClearDatabase();
        SeedDatabase();
        
        var response = await TestHttpClient.GetAsync("/Discounts/1");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadFromJsonAsync<DiscountModel>();
        Assert.That(content, Is.Not.Null);
        Assert.That(content.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task GetDiscountById_Failure()
    {
        ClearDatabase();
        
        var response =  await TestHttpClient.GetAsync("/Discounts/200");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
    
    [Test]
    public async Task DeleteDiscount_Success()
    {
        ClearDatabase();
        SeedDatabase();
        
        var response =  await TestHttpClient.DeleteAsync("/Discounts/1/delete");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task DeleteDiscount_Failure()
    {
        ClearDatabase();
        SeedDatabase();
        
        var response =  await TestHttpClient.DeleteAsync("/Discounts/999/delete");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task UpdateDiscount_Success()
    {
        ClearDatabase();
        SeedDatabase();

        var request = new UpdateDiscountRequest
        {
            Name = "Updated"
        };
        var query = $"?Name={request.Name}";
        var response =  await TestHttpClient.PatchAsync($"/Discounts/1/edit{query}", null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var content =  await response.Content.ReadFromJsonAsync<DiscountModel>();
        Assert.That(content.Name, Is.EqualTo(request.Name));
    }
    
    [Test]
    public async Task UpdateDiscount_Failure()
    {
        ClearDatabase();
        SeedDatabase();

        var request = new UpdateDiscountRequest
        {
            Name = "NAME THAT MORE THAN ONE HUNDRED SYMBOLS IDK WHEN IT WOULD BE ONE HUNDRED SYMBOLS, I THINK I NEED MORE TYPE WORDS TO MAKE THIS STRING MORE THAN ONE HUNDRED SYMBOLS"
        };
        var query = $"?Name={request.Name}";
        var response =  await TestHttpClient.PatchAsync($"/Discounts/1/edit{query}", null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task ChangeDiscountStatus_Success()
    {
        ClearDatabase();
        SeedDatabase();

        var response = await TestHttpClient.PatchAsync("/Discounts/1/edit/status/2", null);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var content = await response.Content.ReadFromJsonAsync<DiscountModel>();
        Assert.That(content.Id, Is.EqualTo(1));
        Assert.That(content.Status.Id, Is.EqualTo(2));
    }
}