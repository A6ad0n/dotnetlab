using System.Security.Claims;
using System.Text;
using AutoMapper;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaApp.BL.Features.Users.DTOs;
using PizzaApp.BL.Features.Users.Entities;
using PizzaApp.BL.Features.Users.Managers;
using PizzaApp.BL.Features.Users.Providers;
using PizzaApp.BL.Features.Users.Validators;
using PizzaApp.WebApi.Controllers.Users.DTOs;
using PizzaApp.WebApi.Controllers.Users.Entities;

namespace PizzaApp.WebApi.Controllers.Users;

[ApiController]
[Route("[controller]")]
public class UsersController(
    IUsersProvider usersProvider,
    IUsersManager usersManager,
    IMapper mapper,
    ILogger<UsersController> logger)
    : ControllerBase
{
    [HttpGet]
    [Route("me")]
    [Authorize]
    public async Task<IActionResult> GetMe()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userModel = await usersProvider.GetByIdAsync(userId);
        return Ok(userModel);
    }

    [HttpPatch]
    [Route("me/edit")]
    [Authorize]
    public async Task<IActionResult> UpdateMe([FromBody] UpdateUserRequest request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var userModel = await usersManager.UpdateUserAsync(userId, mapper.Map<UpdateUserModel>(request));
        return Ok(userModel);
    }
    
    [HttpGet]
    [Route("")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers()
    {
        var usersModels =  await usersProvider.GetAllAsync();
        if (usersModels.IsNullOrEmpty()) return NotFound();
        return Ok(usersModels);
    }
    
    [HttpGet]
    [Route("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserById([FromRoute] int id)
    {
        var userModel = await usersProvider.GetByIdAsync(id);
        return Ok(userModel);
    }

    [HttpDelete]
    [Route("{id:int}/delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser([FromRoute] int id)
    {
        var result = await usersManager.DeleteUserAsync(id);
        if (!result)  return NotFound();
        return Ok();
    }
    
    [HttpPatch]
    [Route("{id:int}/edit/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeUserRole([FromRoute] int id, [FromQuery] ChangeUserRolesRequest request)
    {
        var userModel = await usersManager.ChangeUserRolesAsync(id, mapper.Map<UpdateUserRolesModel>(request));
        return Ok(userModel);
    }
    
    [HttpPatch]
    [Route("{id:int}/edit/block")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeUserBlockInfo([FromRoute] int id, [FromQuery] ChangeUserBlockInfoRequest request)
    {
        var userModel = await usersManager.ChangeBlockInfoUserAsync(id, mapper.Map<BlockInformationModel>(request));
        return Ok(userModel);
    }
}