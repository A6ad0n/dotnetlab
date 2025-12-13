using System.Net;
using System.Reflection;
using System.ComponentModel;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PizzaApp.BL.Common.Exceptions;
using PizzaApp.BL.Features.Auth.Exceptions;
using PizzaApp.BL.Features.Discounts.Exceptions;
using PizzaApp.BL.Features.Menu.Exceptions;
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
        catch (BusinessLogicException<UserResultCode> ex)
        {
            logger.LogWarning(ex, "Business logic exception from PizzaApp.BL.Users: {ExMessage}", ex.Message);

            context.Response.StatusCode = MapStatusCode(ex.ResultCode);
            context.Response.ContentType = "application/json";

            var payload = new
            {
                code = Convert.ToInt32(ex.ResultCode),
                error = ex.Message
            };

            await context.Response.WriteAsJsonAsync(payload);
        }
        catch (BusinessLogicException<MenuResultCode> ex)
        {
            logger.LogWarning(ex, "Business logic exception from PizzaApp.BL.Menu {ExMessage}", ex.Message);

            context.Response.StatusCode = MapStatusCode(ex.ResultCode);
            context.Response.ContentType = "application/json";

            var payload = new
            {
                code = Convert.ToInt32(ex.ResultCode),
                error = ex.Message
            };

            await context.Response.WriteAsJsonAsync(payload);
        }
        catch (BusinessLogicException<DiscountResultCode> ex)
        {
            logger.LogWarning(ex, "Business logic exception from PizzaApp.BL.Users {ExMessage}", ex.Message);

            context.Response.StatusCode = MapStatusCode(ex.ResultCode);
            context.Response.ContentType = "application/json";

            var payload = new
            {
                code = Convert.ToInt32(ex.ResultCode),
                error = ex.Message
            };

            await context.Response.WriteAsJsonAsync(payload);
        }
        catch (Exception ex)
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
            
            _ => StatusCodes.Status400BadRequest
        };
    }
}
