using System.ComponentModel;

namespace PizzaApp.BL.Features.Statuses.Exceptions;

public enum StatusResultCode
{
    [Description("Status not found.")]
    StatusNotFound = 4001,
}