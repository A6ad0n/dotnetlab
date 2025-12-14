using AutoMapper;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaApp.BL.Features.Discounts.DTOs;
using PizzaApp.BL.Features.Discounts.Managers;
using PizzaApp.BL.Features.Discounts.Providers;
using PizzaApp.WebApi.Controllers.v2.Discounts.DTOs;
using PizzaApp.WebApi.Controllers.v2.Discounts.DTOs.Requests;
using PizzaApp.WebApi.Controllers.v2.Discounts.DTOs.Responses;

namespace PizzaApp.WebApi.Controllers.v2.Discounts;

[ApiController]
[Route("api/v2/[controller]")]
[ApiVersion("2.0")]
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
    [Route("{id:guid}")]
    public async Task<IActionResult> GetDiscountById([FromRoute] Guid id)
    {
        var discountModel = await discountProvider.GetByGuidAsync(id);
        return Ok(discountModel);
    }

    [HttpPut]
    [Route("create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateDiscount([FromBody] CreateDiscountRequest request)
    {
        var discountModel = await discountManager.CreateDiscountAsync(mapper.Map<CreateDiscountModel>(request));
        return Ok(discountModel);
    }

    [HttpDelete]
    [Route("{id:guid}/delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteDiscount([FromRoute] Guid id)
    {
        var result = await discountManager.DeleteDiscountAsync(id);
        if (!result)  return NotFound();
        return Ok();
    }
    
    [HttpPatch]
    [Route("{id:guid}/edit")]
    [Authorize (Roles = "Admin")]
    public async Task<IActionResult> UpdateDiscount([FromRoute] Guid id, [FromBody] UpdateDiscountRequest request)
    {
        var discountModel = await discountManager.UpdateDiscountAsync(id, mapper.Map<UpdateDiscountModel>(request));
        return Ok(discountModel);
    }
    
    [HttpPatch]
    [Route("{id:guid}/edit/status/")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ChangeDiscountStatus([FromRoute] Guid id, [FromBody] ChangeDiscountStatusRequest request)
    {
        var discountModel = await discountManager.ChangeDiscountStatusAsync(id, request.StatusGuid);
        return Ok(discountModel);
    }
}