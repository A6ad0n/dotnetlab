using AutoMapper;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Mvc;
using PizzaApp.BL.Features.Categories.Providers;
using PizzaApp.WebApi.Controllers.v1.Categories.DTOs;

namespace PizzaApp.WebApi.Controllers.v1.Categories;

[ApiController]
[Route("api/v1/[controller]")]
public class CategoriesController(
    ICategoryProvider discountProvider,
    IMapper mapper,
    ILogger<CategoriesController> logger)
    : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetCategories()
    {
        var discountModels = await discountProvider.GetAllAsync();
        if (discountModels.IsNullOrEmpty()) return NotFound();
        return Ok(mapper.Map<CategoryListResponse>(discountModels));
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] int id)
    {
        var discountModel = await discountProvider.GetByIdAsync(id);
        return Ok(discountModel);
    }
}