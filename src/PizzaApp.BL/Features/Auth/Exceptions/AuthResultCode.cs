using System.ComponentModel;

namespace PizzaApp.BL.Features.Auth.Exceptions;

public enum AuthResultCode
{
    [Description("User not found.")]
    UserNotFound = 2001,
    
    [Description("User already exists.")]
    UserAlreadyExists = 2002,

    [Description("Email or password is incorrect.")]
    EmailOrPasswordIsIncorrect = 2003,
    
    [Description("User creation failure.")]
    UserCreationFailure = 2004,
    
    [Description("Refresh token is required.")]
    RefreshTokenIsRequired = 2005,
    
    [Description("Authorize validation failure.")]
    AuthorizeValidationFailure = 2006,
    
    [Description("Register validation failure.")]
    RegisterValidationFailure = 2009,  
}