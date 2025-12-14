using AutoMapper;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Mvc;
using PizzaApp.BL.Features.Statuses.Providers;
using PizzaApp.WebApi.Controllers.v2.Statuses.DTOs;

namespace PizzaApp.WebApi.Controllers.v2.Statuses;

[ApiController]
[Route("api/v2/[controller]")]
[ApiVersion("2.0")]
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
    [Route("{id:guid}")]
    public async Task<IActionResult> GetStatusById([FromRoute] Guid id)
    {
        var statusModel = await statusProvider.GetByGuidAsync(id);
        return Ok(statusModel);
    }
}