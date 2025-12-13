using FluentValidation;
using PizzaApp.BL.Features.Auth.Entities;

namespace PizzaApp.BL.Features.Auth.Validators;

public class RegisterUserModelValidator : AbstractValidator<RegisterUserModel>
{
    public RegisterUserModelValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email address is required")
            .EmailAddress()
            .WithMessage("Email address is invalid")
            .MaximumLength(255)
            .WithMessage("Email must be less than 255 characters");
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .Matches(@"^7\d{10}$")
            .WithMessage("Phone number is invalid");

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Login is required")
            .MaximumLength(50)
            .WithMessage("Login must be less than 50 characters");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MaximumLength(50)
            .WithMessage("Password must be less than 255 characters");
    }
    
}