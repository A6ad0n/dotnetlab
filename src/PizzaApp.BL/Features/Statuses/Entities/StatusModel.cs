using PizzaApp.BL.Common.Primitives;

namespace PizzaApp.BL.Features.Statuses.Entities;

public class StatusModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public StatusTypeModel StatusType { get; set; }
}