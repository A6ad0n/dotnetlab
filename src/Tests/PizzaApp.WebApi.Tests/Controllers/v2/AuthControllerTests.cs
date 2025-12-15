using System.Net;
using System.Net.Http.Json;
using PizzaApp.BL.Features.Auth.Entities;
using PizzaApp.WebApi.Controllers.v1.Users.DTOs;
using PizzaApp.WebApi.Controllers.v2.Authorization.DTOs;
using PizzaApp.WebApi.Controllers.v2.Users.DTOs.Responses;

namespace PizzaApp.WebApi.Tests.Controllers.v2;

public class AuthControllerTests : TestBase
{
    private const string ValidUsername = "testusertest";
    private const string ValidEmail = "testing@test.test";
    private const string ValidPhone = "79991234567";
    private const string ValidPassword = "CorrectP@ssw0rd1";
    
    private const string EmptyString = "";
    private const string InvalidEmail = "invalidemail";
    private const string EmailWithoutAt = "emailwithoutat.com";
    private const string EmailWithoutDomain = "test@";
    private const string PhoneNotStartingWith7 = "89991234567";
    private const string ShortPhone = "7999123456";
    private const string LongPhone = "799912345678";
    private const string PhoneWithLetters = "7abcdefghij";
    private const string WeakPassword = "weak";
    
    protected override async Task AdditionalOneTimeSetUp()
    {
        var request = new RegisterUserRequest
        {
            UserName = "testuser",
            Email = "test@test.test",
            PhoneNumber = "79999999999",
            Password = "P@ssw0rd"
        };
        
        var response = await TestHttpClient.PostAsJsonAsync(
            PizzaAppEndpoints.v2.RegisterUser, request);
        var content = await response.Content.ReadFromJsonAsync<UserResponse>(); 
    }
    
    
    [Test]
    public async Task Login_Success()
    {
        var request = new AuthorizeUserRequest
        {
            Email = "test@test.test",
            Password = "P@ssw0rd"
        };

        var response = await TestHttpClient.PostAsJsonAsync(
            PizzaAppEndpoints.v2.LoginUser, request);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Login_InvalidCredentials_Failure()
    {
        var request = new AuthorizeUserRequest
        {
            Email = "test@test.test",
            Password = ValidPassword
        };
        
        var response = await TestHttpClient.PostAsJsonAsync(
            PizzaAppEndpoints.v2.LoginUser, request);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
    
    [Test]
    public async Task Login_UserNotFound_Failure()
    {
        var request = new AuthorizeUserRequest
        {
            Email = "neverusedmail@forregistration.esketit",
            Password = ValidPassword
        };
        
        var response = await TestHttpClient.PostAsJsonAsync(
            PizzaAppEndpoints.v2.LoginUser, request);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    [TestCase(EmptyString, EmptyString)]
    [TestCase(ValidEmail, EmptyString)]
    [TestCase(InvalidEmail, EmptyString)]
    [TestCase(InvalidEmail,ValidPassword)]
    [TestCase(InvalidEmail,WeakPassword)]
    public async Task Login_Validation_Failure(string email, string password)
    {
        var request = new AuthorizeUserRequest
        {
            Email = email,
            Password = password
        };
        
        var response = await TestHttpClient.PostAsJsonAsync(
            PizzaAppEndpoints.v2.LoginUser, request);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Refresh_Success()
    {
        var loginRequest = new AuthorizeUserRequest
        {
            Email = "test@test.test",
            Password = "P@ssw0rd"
        };
        var loginResponse = await TestHttpClient.PostAsJsonAsync(
            PizzaAppEndpoints.v2.LoginUser, loginRequest);
        var content = await loginResponse.Content.ReadFromJsonAsync<TokensResponse>(); 
        var request = new RefreshTokenRequest
        {
            RefreshToken = content!.RefreshToken!
        };
        
        var response = await TestHttpClient.PostAsJsonAsync(
            PizzaAppEndpoints.v2.RefreshToken, request);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Refresh_Failure()
    {
        var request = new RefreshTokenRequest
        {
            RefreshToken = "invalid"
        };
        
        var response = await TestHttpClient.PostAsJsonAsync(
            PizzaAppEndpoints.v2.RefreshToken, request);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
    
    [Test]
    public async Task Refresh_Validation_Failure()
    {
        var request = new RefreshTokenRequest
        {
            RefreshToken = ""
        };
        
        var response = await TestHttpClient.PostAsJsonAsync(
            PizzaAppEndpoints.v2.RefreshToken, request);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Register_Success()
    {
        var request = new RegisterUserRequest
        {
            UserName = "newuser",
            Email = "newuser@test.com",
            PhoneNumber = "70000000000",
            Password = "P@ssw0rd"
        };
        
        var response = await TestHttpClient.PostAsJsonAsync(
            PizzaAppEndpoints.v2.RegisterUser, request);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task Register_Failure()
    {
        var request = new RegisterUserRequest
        {
            UserName = "userthatalreadyexists",
            Email = "test@test.test",
            PhoneNumber = "70000000001",
            Password = "AnotherP@ssw0rd"
        };
        
        var response = await TestHttpClient.PostAsJsonAsync(
            PizzaAppEndpoints.v2.RegisterUser, request);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test][TestCase(ValidUsername, EmptyString, ValidPhone, ValidPassword)]
    [TestCase(ValidUsername, InvalidEmail, ValidPhone, ValidPassword)]
    [TestCase(ValidUsername, EmailWithoutAt, ValidPhone, ValidPassword)]
    [TestCase(ValidUsername, EmailWithoutDomain, ValidPhone, ValidPassword)]
    [TestCase(ValidUsername, ValidEmail, EmptyString, ValidPassword)]
    [TestCase(ValidUsername, ValidEmail, PhoneNotStartingWith7, ValidPassword)]
    [TestCase(ValidUsername, ValidEmail, ShortPhone, ValidPassword)]
    [TestCase(ValidUsername, ValidEmail, LongPhone, ValidPassword)]
    [TestCase(ValidUsername, ValidEmail, PhoneWithLetters, ValidPassword)]
    [TestCase(EmptyString, ValidEmail, ValidPhone, ValidPassword)]
    [TestCase(ValidUsername, ValidEmail, ValidPhone, EmptyString)]
    [TestCase(EmptyString, EmptyString, EmptyString, EmptyString)]
    [TestCase(ValidUsername, InvalidEmail, ValidPhone, WeakPassword)]
    [TestCase(EmptyString, ValidEmail, PhoneWithLetters, ValidPassword)]
    public async Task Register_Validation_Failure(string username, string email, string phoneNumber, string password)
    {
        var request = new RegisterUserRequest
        {
            UserName = username,
            Email = email,
            PhoneNumber = phoneNumber,
            Password = password
        };
        
        var response = await TestHttpClient.PostAsJsonAsync(
            PizzaAppEndpoints.v2.RegisterUser, request);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}