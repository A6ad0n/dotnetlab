using PizzaApp.BL.Features.Statuses.Entities;

namespace PizzaApp.WebApi.Controllers.v1.Statuses.DTOs;

public class StatusListResponse
{
    public List<StatusModel> Statuses { get; set; }
}