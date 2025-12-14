using AutoMapper;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Mvc;
using PizzaApp.BL.Features.Categories.Providers;
using PizzaApp.WebApi.Controllers.v2.Categories.DTOs;

namespace PizzaApp.WebApi.Controllers.v2.Categories;

[ApiController]
[Route("api/v2/[controller]")]
[ApiVersion("2.0")]
public class CategoriesController(
    ICategoryProvider categoryProvider,
    IMapper mapper,
    ILogger<CategoriesController> logger)
    : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetCategories()
    {
        var categoryModels = await categoryProvider.GetAllAsync();
        if (categoryModels.IsNullOrEmpty()) return NotFound();
        return Ok(mapper.Map<CategoryListResponse>(categoryModels));
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] int id)
    {
        var categoryModel = await categoryProvider.GetByIdAsync(id);
        return Ok(categoryModel);
    }
}