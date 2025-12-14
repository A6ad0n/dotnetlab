using System.Net;
using System.Reflection;
using System.ComponentModel;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PizzaApp.BL.Common.Exceptions;
using PizzaApp.BL.Features.Auth.Exceptions;
using PizzaApp.BL.Features.Categories.Exceptions;
using PizzaApp.BL.Features.Discounts.Exceptions;
using PizzaApp.BL.Features.Menu.Exceptions;
using PizzaApp.BL.Features.Statuses.Exceptions;
using PizzaApp.BL.Features.Users.Exceptions;

namespace PizzaApp.WebApi.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        switch (ex)
        {
            case BusinessLogicException<AuthResultCode> businessEx:
                await HandleBusinessLogicException(context, businessEx, "PizzaApp.BL.Auth");
                break;
            
            case BusinessLogicException<UserResultCode> businessEx:
                await HandleBusinessLogicException(context, businessEx, "PizzaApp.BL.Users");
                break;
            
            case BusinessLogicException<CommonResultCode> businessEx:
                await HandleBusinessLogicException(context, businessEx, "Common resultCode");
                break;
            
            case BusinessLogicException<MenuResultCode> businessEx:
                await HandleBusinessLogicException(context, businessEx, "PizzaApp.BL.Menu");
                break;
            
            case BusinessLogicException<DiscountResultCode> businessEx:
                await HandleBusinessLogicException(context, businessEx, "PizzaApp.BL.Discounts");
                break;
            
            case BusinessLogicException<CategoryResultCode> businessEx:
                await HandleBusinessLogicException(context, businessEx, "PizzaApp.BL.Categories");
                break;
            
            case BusinessLogicException<StatusResultCode> businessEx:
                await HandleBusinessLogicException(context, businessEx, "PizzaApp.BL.Statuses");
                break;
            
            default:
                await HandleException(context, ex);
                break;
        }
    }
    
    private async Task HandleBusinessLogicException<TResultCode>(HttpContext context, 
        BusinessLogicException<TResultCode> ex, string source) where TResultCode : Enum
    {
        logger.LogWarning(ex, "Business logic exception from {Source}: {ExMessage}", source, ex.Message);

        context.Response.StatusCode = MapStatusCode(ex.ResultCode);
        context.Response.ContentType = "application/json";

        var payload = new
        {
            code = Convert.ToInt32(ex.ResultCode),
            error = ex.Message
        };

        await context.Response.WriteAsJsonAsync(payload);
    }
    
    private async Task HandleException(HttpContext context, Exception ex)
    {
        logger.LogError(ex, "Unhandled exception {ExMessage}", ex.Message);

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var payload = new
        {
            code = 0,
            error = "Unexpected error occurred",
            detail = ex.Message
        };

        await context.Response.WriteAsJsonAsync(payload);
    }

    private static int MapStatusCode(Enum code)
    {
        return code switch
        {
            // AuthResultCode
            AuthResultCode.UserNotFound => StatusCodes.Status404NotFound,
            AuthResultCode.UserAlreadyExists => StatusCodes.Status400BadRequest,
            AuthResultCode.EmailOrPasswordIsIncorrect => StatusCodes.Status401Unauthorized,
            AuthResultCode.UserCreationFailure => StatusCodes.Status500InternalServerError,
            AuthResultCode.AuthorizeValidationFailure => StatusCodes.Status400BadRequest,
            AuthResultCode.RegisterValidationFailure => StatusCodes.Status400BadRequest,
            
            // UserResultCode
            UserResultCode.UserNotFound => StatusCodes.Status404NotFound,
            UserResultCode.RolesNotFound => StatusCodes.Status404NotFound,
            UserResultCode.UserAlreadyExists => StatusCodes.Status400BadRequest,
            UserResultCode.EmailOrPasswordIsIncorrect => StatusCodes.Status401Unauthorized,
            UserResultCode.UserCreationFailure => StatusCodes.Status500InternalServerError,
            UserResultCode.UserUpdateFailure => StatusCodes.Status500InternalServerError,
            UserResultCode.UserValidationFailure => StatusCodes.Status400BadRequest,
            
            // CommonResultCode
            CommonResultCode.IdentityServerError => StatusCodes.Status500InternalServerError,
            CommonResultCode.ValidationFailed => StatusCodes.Status400BadRequest,
            CommonResultCode.RequiredFieldMissing => StatusCodes.Status400BadRequest,
            CommonResultCode.InvalidInput => StatusCodes.Status400BadRequest,
            
            // MenuResultCode
            MenuResultCode.MenuItemNotFound => StatusCodes.Status404NotFound,
            MenuResultCode.DiscountsNotFound => StatusCodes.Status404NotFound,
            MenuResultCode.MenuItemAlreadyExists => StatusCodes.Status400BadRequest,
            MenuResultCode.MenuItemCreationFailure => StatusCodes.Status500InternalServerError,
            MenuResultCode.MenuItemUpdateFailure => StatusCodes.Status500InternalServerError,
            MenuResultCode.MenuItemValidationFailure => StatusCodes.Status400BadRequest,
            
            // DiscountResultCode
            DiscountResultCode.DiscountNotFound => StatusCodes.Status404NotFound,
            DiscountResultCode.DiscountAlreadyExists => StatusCodes.Status400BadRequest,
            DiscountResultCode.DiscountCreationFailure => StatusCodes.Status500InternalServerError,
            DiscountResultCode.DiscountUpdateFailure => StatusCodes.Status500InternalServerError,
            DiscountResultCode.DiscountValidationFailure => StatusCodes.Status400BadRequest,
            
            // CategoryResultCode
            CategoryResultCode.CategoryNotFound => StatusCodes.Status404NotFound,
            // StatusResultCode
            StatusResultCode.StatusNotFound => StatusCodes.Status404NotFound,
            
            _ => StatusCodes.Status400BadRequest
        };
    }
}
