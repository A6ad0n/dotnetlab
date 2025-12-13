using System.ComponentModel;

namespace PizzaApp.BL.Common.Exceptions;

public enum CommonResultCode
{
    [Description("Identity server error.")]
    IdentityServerError = 1000,
    
    [Description("Unexpected error.")]
    UnexpectedError = 1001,
    
    [Description("Operation timeout.")]
    OperationTimeout = 1002,

    [Description("Database error.")]
    DatabaseError = 1003,
    
    [Description("Validation failed.")]
    ValidationFailed = 1200,
    
    [Description("Invalid input data.")]
    InvalidInput = 1201,
    
    [Description("Required field is missing.")]
    RequiredFieldMissing = 1202,
}
