using AutoMapper;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Mvc;
using PizzaApp.BL.Features.Statuses.Providers;
using PizzaApp.WebApi.Controllers.Statuses.DTOs;

namespace PizzaApp.WebApi.Controllers.Statuses;

[ApiController]
[Route("[controller]")]
public class StatusesController(
    IStatusProvider discountProvider,
    IMapper mapper,
    ILogger<StatusesController> logger)
    : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetStatuses()
    {
        var discountModels = await discountProvider.GetAllAsync();
        if (discountModels.IsNullOrEmpty()) return NotFound();
        return Ok(mapper.Map<StatusListResponse>(discountModels));
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetStatusById([FromRoute] int id)
    {
        var discountModel = await discountProvider.GetByIdAsync(id);
        return Ok(discountModel);
    }
}