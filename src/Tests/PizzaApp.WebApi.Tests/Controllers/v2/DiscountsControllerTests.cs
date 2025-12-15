using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Npgsql;
using PizzaApp.WebApi.Controllers.v2.Discounts.DTOs.Requests;
using PizzaApp.WebApi.Controllers.v2.Discounts.DTOs.Responses;
using PizzaApp.WebApi.Tests.Helpers;

namespace PizzaApp.WebApi.Tests.Controllers.v2;

public class DiscountsControllerTests : TestBase
{
    protected override async Task AdditionalOneTimeSetUp()
    {
        await SeedDatabaseAsync();
    }


    [Test]
    public async Task GetDiscounts_Success()
    {
        var response = await TestHttpClient.GetAsync(PizzaAppEndpoints.v2.Discounts);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadFromJsonAsync<DiscountListResponse>();
        Assert.That(content, Is.Not.Null);
    }

    [Test]
    public async Task GetDiscountById_Success()
    {
        var response = await TestHttpClient.GetAsync(
            $"{PizzaAppEndpoints.v2.Discounts}/11110000-0000-0000-0000-000000000000");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var content = await response.Content.ReadFromJsonAsync<DiscountResponse>();
        Assert.That(content, Is.Not.Null);
        Assert.That(content.Id, Is.EqualTo(new Guid("11110000-0000-0000-0000-000000000000")));
    }

    [Test]
    public async Task GetDiscountById_Failure()
    {
        var response =  await TestHttpClient.GetAsync(
            $"{PizzaAppEndpoints.v2.Discounts}/11110000-0000-0000-0000-000056900000");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
    
    
    [Test]
    public async Task DeleteDiscount_WithoutAuth_Failure()
    {
        var response =  await TestHttpClient.DeleteAsync(
            $"{PizzaAppEndpoints.v2.Discounts}/22220000-0000-0000-0000-000000000000/delete");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
    
    [Test]
    public async Task DeleteDiscount_WithAuth_Success()
    {
        var accessToken = await GetAdminAccessTokenAsync();
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete,
            $"{PizzaAppEndpoints.v2.Discounts}/22220000-0000-0000-0000-000000000000/delete");
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        var response =  await TestHttpClient.SendAsync(requestMessage);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task DeleteDiscount_WithAuth_Failure()
    {
        var accessToken = await GetAdminAccessTokenAsync();
        var requestMessage = new HttpRequestMessage(HttpMethod.Delete,
            $"{PizzaAppEndpoints.v2.Discounts}/11110000-0000-0000-0000-432000000000/delete");
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        var response =  await TestHttpClient.SendAsync(requestMessage);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task UpdateDiscount_WithoutAuth_Failure()
    {
        var request = new UpdateDiscountRequest
        {
            Name = "Updated"
        };
        var response = await TestHttpClient.PatchAsJsonAsync(
            $"{PizzaAppEndpoints.v2.Discounts}/11110000-0000-0000-0000-000000000000/edit", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }

    [Test]
    public async Task UpdateDiscount_WithAuth_Success()
    {
        var accessToken = await GetAdminAccessTokenAsync();
        var request = new UpdateDiscountRequest
        {
            Name = "Updated"
        };
        var requestMessage = new HttpRequestMessage(HttpMethod.Patch,
            $"{PizzaAppEndpoints.v2.Discounts}/11110000-0000-0000-0000-000000000000/edit")
        {
            Content = JsonContent.Create(request)
        };
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        var response =  await TestHttpClient.SendAsync(requestMessage);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var content =  await response.Content.ReadFromJsonAsync<DiscountResponse>();
        Assert.That(content.Name, Is.EqualTo(request.Name));
    }
    
    [Test]
    public async Task UpdateDiscount_WithAuth_Failure()
    {
        var accessToken = await GetAdminAccessTokenAsync();
        var request = new UpdateDiscountRequest
        {
            Name = "NAME THAT MORE THAN ONE HUNDRED SYMBOLS IDK WHEN IT WOULD BE ONE HUNDRED SYMBOLS, I THINK I NEED MORE TYPE WORDS TO MAKE THIS STRING MORE THAN ONE HUNDRED SYMBOLS"
        };
        var requestMessage = new HttpRequestMessage(HttpMethod.Patch,
            $"{PizzaAppEndpoints.v2.Discounts}/11110000-0000-0000-0000-000000000000/edit")
        {
            Content = JsonContent.Create(request)
        };
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        var response =  await TestHttpClient.SendAsync(requestMessage);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    
    [Test]
    public async Task ChangeDiscountStatus_WithoutAuth_Failure()
    {
        var request = new ChangeDiscountStatusRequest
        {
            StatusGuid = new Guid("20000000-0000-0000-0000-000000000000")
        };
        var response = await TestHttpClient.PatchAsJsonAsync(
            $"{PizzaAppEndpoints.v2.Discounts}" +
            "/11110000-0000-0000-0000-000000000000/edit/status", request);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
    
    [Test]
    public async Task ChangeDiscountStatus_WithAuth_Success()
    {
        var accessToken = await GetAdminAccessTokenAsync();
        var request = new ChangeDiscountStatusRequest
        {
            StatusGuid = new Guid("20000000-0000-0000-0000-000000000000")
        };
        var requestMessage = new HttpRequestMessage(HttpMethod.Patch,
            $"{PizzaAppEndpoints.v2.Discounts}/11110000-0000-0000-0000-000000000000/edit/status")
        {
            Content = JsonContent.Create(request)
        };
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);
        
        var response = await TestHttpClient.SendAsync(requestMessage);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var content = await response.Content.ReadFromJsonAsync<DiscountResponse>();
        Assert.That(content.Id, Is.EqualTo(new Guid("11110000-0000-0000-0000-000000000000")));
        Assert.That(content.Status.Id, Is.EqualTo(new Guid("20000000-0000-0000-0000-000000000000")));
    }
    
    [Test]
    public async Task ChangeDiscountStatus_WithAuth_Failure()
    {
        var accessToken = await GetAdminAccessTokenAsync();
        var request = new ChangeDiscountStatusRequest
        {
            StatusGuid = new Guid("20000000-0000-0000-0000-123000000000")
        };
        var requestMessage = new HttpRequestMessage(HttpMethod.Patch,
            $"{PizzaAppEndpoints.v2.Discounts}/11110000-0000-0000-0000-000000000000/edit/status")
        {
            Content = JsonContent.Create(request)
        };
        requestMessage.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await TestHttpClient.SendAsync(requestMessage);
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}