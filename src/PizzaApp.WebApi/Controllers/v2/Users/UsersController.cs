using System.Security.Claims;
using AutoMapper;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaApp.BL.Features.Users.DTOs;
using PizzaApp.BL.Features.Users.Entities;
using PizzaApp.BL.Features.Users.Managers;
using PizzaApp.BL.Features.Users.Providers;
using PizzaApp.WebApi.Controllers.v2.Users.DTOs;
using PizzaApp.WebApi.Controllers.v2.Users.DTOs.Requests;
using PizzaApp.WebApi.Controllers.v2.Users.DTOs.Responses;

namespace PizzaApp.WebApi.Controllers.v2.Users;

[ApiController]
[Route("api/v2/[controller]")]
[ApiVersion("2.0")]
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
        return Ok(mapper.Map<UserListResponse>(usersModels));
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var userModel = await usersProvider.GetByGuidAsync(id);
        return Ok(userModel);
    }

    [HttpDelete]
    [Route("{id:guid}/delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var result = await usersManager.DeleteUserAsync(id);
        if (!result)  return NotFound();
        return Ok();
    }
    
    [HttpPatch]
    [Route("{id:guid}/edit/roles")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeUserRole([FromRoute] Guid id, [FromBody] ChangeUserRolesRequest request)
    {
        var userModel = await usersManager.ChangeUserRolesAsync(id, request.RoleGuids);
        return Ok(userModel);
    }
    
    [HttpPatch]
    [Route("{id:guid}/edit/block")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeUserBlockInfo([FromRoute] Guid id, [FromBody] ChangeUserBlockInfoRequest request)
    {
        var userModel = await usersManager.ChangeBlockInfoUserAsync(id, mapper.Map<BlockInformationModel>(request));
        return Ok(userModel);
    }
}