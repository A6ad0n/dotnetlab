using System.ComponentModel;

namespace PizzaApp.BL.Features.Menu.Exceptions;

public enum MenuResultCode
{
    [Description("MenuItem not found.")]
    MenuItemNotFound = 4001,
    
    [Description("MenuItem already exists.")]
    MenuItemAlreadyExists = 4002,
    
    [Description("MenuItem creation failure.")]
    MenuItemCreationFailure = 4004,
    
    [Description("MenuItem updating failed.")]
    MenuItemUpdateFailure = 4005,
    
    [Description("Category not found.")]
    CategoryNotFound = 4006,    
    
    [Description("Status not found.")]
    StatusNotFound = 4007,    
    
    [Description("Discount not found.")]
    DiscountsNotFound = 4008,
    
    [Description("Discount validation failure.")]
    MenuItemValidationFailure = 4009,  
}