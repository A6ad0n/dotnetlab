using FluentValidation;
using PizzaApp.BL.Features.Users.DTOs;

namespace PizzaApp.BL.Features.Users.Validators;

public class UpdateUserModelValidator : AbstractValidator<UpdateUserModel>
{
    public UpdateUserModelValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email address is invalid")
            .MaximumLength(255)
            .WithMessage("Email must be less than 255 characters");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^7\d{10}$")
            .WithMessage("Phone number is invalid");

        RuleFor(x => x.UserName)
            .MaximumLength(50)
            .WithMessage("Login must be less than 50 characters");

    }
}