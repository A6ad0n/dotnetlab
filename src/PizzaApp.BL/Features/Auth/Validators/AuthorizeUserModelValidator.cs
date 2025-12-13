using FluentValidation;
using PizzaApp.BL.Features.Auth.Entities;

namespace PizzaApp.BL.Features.Auth.Validators;

public class AuthorizeUserModelValidator : AbstractValidator<AuthorizeUserModel>
{
    public AuthorizeUserModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email address is required")
            .EmailAddress()
            .WithMessage("Email address is invalid")
            .MaximumLength(255)
            .WithMessage("Email must be less than 255 characters");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MaximumLength(50)
            .WithMessage("Password must be less than 255 characters");
    }
}