using System.Security.Claims;
using System.Text;
using AutoMapper;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaApp.BL.Features.Discounts.DTOs;
using PizzaApp.BL.Features.Discounts.Managers;
using PizzaApp.BL.Features.Discounts.Providers;
using PizzaApp.WebApi.Controllers.Discounts.Entities;

namespace PizzaApp.WebApi.Controllers.Discounts;

[ApiController]
[Route("[controller]")]
public class DiscountsController(
    IDiscountProvider discountProvider,
    IDiscountManager discountManager,
    IMapper mapper,
    ILogger<DiscountsController> logger)
    : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetDiscounts()
    {
        var discountModels = await discountProvider.GetAllAsync();
        if (discountModels.IsNullOrEmpty()) return NotFound();
        return Ok(mapper.Map<DiscountListResponse>(discountModels));
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetDiscountById([FromRoute] int id)
    {
        var discountModel = await discountProvider.GetByIdAsync(id);
        return Ok(discountModel);
    }

    [HttpPut]
    [Route("create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateDiscount([FromQuery] CreateDiscountRequest request)
    {
        var discountModel = await discountManager.CreateDiscountAsync(mapper.Map<CreateDiscountModel>(request));
        return Ok(discountModel);
    }

    [HttpDelete]
    [Route("{id:int}/delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDiscount([FromRoute] int id)
    {
        var result = await discountManager.DeleteDiscountAsync(id);
        if (!result)  return NotFound();
        return Ok();
    }
    
    [HttpPatch]
    [Route("{id:int}/edit")]
    [Authorize (Roles = "Admin")]
    public async Task<IActionResult> UpdateDiscount([FromRoute] int id, [FromQuery] UpdateDiscountRequest request)
    {
        var discountModel = await discountManager.UpdateDiscountAsync(id, mapper.Map<UpdateDiscountModel>(request));
        return Ok(discountModel);
    }
    
    [HttpPatch]
    [Route("{id:int}/edit/status/{statusId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeDiscountStatus([FromRoute] int id, [FromRoute] int statusId)
    {
        var discountModel = await discountManager.ChangeDiscountStatusAsync(id, statusId);
        return Ok(discountModel);
    }
}