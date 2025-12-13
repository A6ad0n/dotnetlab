using System.ComponentModel;

namespace PizzaApp.BL.Features.Discounts.Exceptions;

public enum DiscountResultCode
{
    [Description("Discount not found.")]
    DiscountNotFound = 3001,
    
    [Description("Discount already exists.")]
    DiscountAlreadyExists = 3002,
    
    [Description("Discount creation failure.")]
    DiscountCreationFailure = 3004,
    
    [Description("Discount updating failed.")]
    DiscountUpdateFailure = 3005,
    
    [Description("Status not found.")]
    StatusNotFound = 3006,    
    
    [Description("Discount validation failure.")]
    DiscountValidationFailure = 3007,    
}