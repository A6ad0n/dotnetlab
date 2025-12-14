using AutoMapper;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Mvc;
using PizzaApp.BL.Features.Statuses.Providers;
using PizzaApp.WebApi.Controllers.v1.Statuses.DTOs;

namespace PizzaApp.WebApi.Controllers.v1.Statuses;

[ApiController]
[Route("api/v1/[controller]")]
[ApiVersion("1.0")]
public class StatusesController(
    IStatusProvider statusProvider,
    IMapper mapper,
    ILogger<StatusesController> logger)
    : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetStatuses()
    {
        var statusModels = await statusProvider.GetAllAsync();
        if (statusModels.IsNullOrEmpty()) return NotFound();
        return Ok(mapper.Map<StatusListResponse>(statusModels));
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetStatusById([FromRoute] int id)
    {
        var statusModel = await statusProvider.GetByIdAsync(id);
        return Ok(statusModel);
    }
}