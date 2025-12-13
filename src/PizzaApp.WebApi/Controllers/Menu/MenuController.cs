using System.Security.Claims;
using System.Text;
using AutoMapper;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaApp.BL.Features.Menu.DTOs;
using PizzaApp.BL.Features.Menu.Managers;
using PizzaApp.BL.Features.Menu.Providers;
using PizzaApp.WebApi.Controllers.Menu.Entities;

namespace PizzaApp.WebApi.Controllers.Menu;

[ApiController]
[Route("[controller]")]
public class MenuController(
    IMenuProvider menuProvider,
    IMenuManager menuManager,
    IMapper mapper,
    ILogger<MenuController> logger)
    : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetMenu()
    {
        var menuModels = await menuProvider.GetAllAsync();
        if (menuModels.IsNullOrEmpty()) return NotFound();
        return Ok(mapper.Map<MenuItemListResponse>(menuModels));
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetMenuItemById([FromRoute] int id)
    {
        var menuItemModel = await menuProvider.GetByIdAsync(id);
        return Ok(menuItemModel);
    }

    [HttpPut]
    [Route("create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateMenuItem([FromQuery] CreateMenuItemRequest request)
    {
        var menuItemModel = await menuManager.CreateMenuItemAsync(mapper.Map<CreateMenuItemModel>(request));
        return Ok(menuItemModel);
    }

    [HttpDelete]
    [Route("{id:int}/delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteMenuItem([FromRoute] int id)
    {
        var result = await menuManager.DeleteMenuItemAsync(id);
        if (!result)  return NotFound();
        return Ok();
    }
    
    [HttpPatch]
    [Route("{id:int}/edit")]
    [Authorize (Roles = "Admin")]
    public async Task<IActionResult> UpdateMenuItem([FromRoute] int id, [FromQuery] UpdateMenuItemRequest request)
    {
        var menuItemModel = await menuManager.UpdateMenuItemAsync(id, mapper.Map<UpdateMenuItemModel>(request));
        return Ok(menuItemModel);
    }
    
    [HttpPatch]
    [Route("{id:int}/edit/discounts")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeMenuItemDiscounts([FromRoute] int id, [FromBody] List<int> discountIds)
    {
        var menuItemModel = await menuManager.ChangeMenuItemDiscountsAsync(id, discountIds);
        return Ok(menuItemModel);
    }
    
    [HttpPatch]
    [Route("{id:int}/edit/category{categoryId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeMenuItemCategory([FromRoute] int id, [FromRoute] int categoryId)
    {
        var menuItemModel = await menuManager.ChangeMenuItemCategoryAsync(id, categoryId);
        return Ok(menuItemModel);
    }
    
    [HttpPatch]
    [Route("{id:int}/edit/status{statusId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeMenuItemStatus([FromRoute] int id, [FromRoute] int statusId)
    {
        var menuItemModel = await menuManager.ChangeMenuItemStatusAsync(id, statusId);
        return Ok(menuItemModel);
    }
}