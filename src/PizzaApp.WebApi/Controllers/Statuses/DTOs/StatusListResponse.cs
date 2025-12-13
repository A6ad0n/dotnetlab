using PizzaApp.BL.Features.Statuses.Entities;

namespace PizzaApp.WebApi.Controllers.Statuses.DTOs;

public class StatusListResponse
{
    List<StatusModel> Statuses { get; set; }
}