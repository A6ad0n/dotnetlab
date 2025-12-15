using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PizzaApp.BL.Features.Auth.Entities;
using PizzaApp.BL.Features.Auth.Managers;
using PizzaApp.WebApi.Controllers.v1.Authorization.DTOs;
using PizzaApp.WebApi.Controllers.v1.Users.DTOs;

namespace PizzaApp.WebApi.Controllers.v1.Authorization;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class AuthController(IAuthManager authManager, IMapper mapper, ILogger<AuthController> logger)
    : ControllerBase
{
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginUser([FromBody] AuthorizeUserRequest request)
    {
        var tokens = await authManager.LoginUserAsync(mapper.Map<AuthorizeUserModel>(request));
        return Ok(tokens);
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var refreshToken = await authManager.RefreshTokenAsync(request.RefreshToken);
        return Ok(refreshToken);
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest request)
    {
        var userModel = await authManager.RegisterUserAsync(mapper.Map<RegisterUserModel>(request));
        return Ok(userModel);
    }
}