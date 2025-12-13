using PizzaApp.BL.Features.Statuses.Entities;

namespace PizzaApp.WebApi.Controllers.Statuses.DTOs;

public class StatusListResponse
{
    public List<StatusModel> Statuses { get; set; }
}