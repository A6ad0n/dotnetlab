using System.ComponentModel;

namespace PizzaApp.BL.Features.Categories.Exceptions;

public enum CategoryResultCode
{
    [Description("Status not found.")]
    CategoryNotFound = 4001,
}