using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PizzaApp.BL.Features.Auth.Entities;
using PizzaApp.BL.Features.Auth.Managers;
using PizzaApp.WebApi.Controllers.v2.Authorization.DTOs;
using PizzaApp.WebApi.Controllers.v2.Users.DTOs;
using PizzaApp.WebApi.Controllers.v2.Users.DTOs.Requests;

namespace PizzaApp.WebApi.Controllers.v2.Authorization;

[ApiController]
[Route("api/v2/[controller]")]
[ApiVersion("2.0")]
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