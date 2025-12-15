using AutoMapper;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaApp.BL.Features.Menu.DTOs;
using PizzaApp.BL.Features.Menu.Managers;
using PizzaApp.BL.Features.Menu.Providers;
using PizzaApp.WebApi.Controllers.v1.Menu.DTOs;

namespace PizzaApp.WebApi.Controllers.v1.Menu;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
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
    public async Task<IActionResult> CreateMenuItem([FromBody] CreateMenuItemRequest request)
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
    public async Task<IActionResult> UpdateMenuItem([FromRoute] int id, [FromBody] UpdateMenuItemRequest request)
    {
        var menuItemModel = await menuManager.UpdateMenuItemAsync(id, mapper.Map<UpdateMenuItemModel>(request));
        return Ok(menuItemModel);
    }
    
    [HttpPatch]
    [Route("{id:int}/edit/discounts")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeMenuItemDiscounts([FromRoute] int id, [FromBody] ChangeMenuItemDiscountsRequest request)
    {
        var menuItemModel = await menuManager.ChangeMenuItemDiscountsAsync(id, request.DiscountIds);
        return Ok(menuItemModel);
    }
    
    [HttpPatch]
    [Route("{id:int}/edit/category")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeMenuItemCategory([FromRoute] int id, [FromBody] ChangeMenuItemCategoryRequest request)
    {
        var menuItemModel = await menuManager.ChangeMenuItemCategoryAsync(id, request.CategoryId);
        return Ok(menuItemModel);
    }
    
    [HttpPatch]
    [Route("{id:int}/edit/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeMenuItemStatus([FromRoute] int id, [FromBody] ChangeMenuItemStatusRequest request)
    {
        var menuItemModel = await menuManager.ChangeMenuItemStatusAsync(id, request.StatusId);
        return Ok(menuItemModel);
    }
}