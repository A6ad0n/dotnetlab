using System.ComponentModel;

namespace PizzaApp.BL.Features.Users.Exceptions;

public enum UserResultCode
{
    [Description("User not found.")]
    UserNotFound = 2001,
    
    [Description("User already exists.")]
    UserAlreadyExists = 2002,

    [Description("Email or password is incorrect.")]
    EmailOrPasswordIsIncorrect = 2003,
    
    [Description("User creation failure.")]
    UserCreationFailure = 2004,
    
    [Description("User updating failed.")]
    UserUpdateFailure = 2005,
    
    [Description("Role not found.")]
    RolesNotFound = 2006,
    
    [Description("Discount validation failure.")]
    UserValidationFailure = 2009,  
}