using FluentValidation;
using PizzaApp.BL.Features.Menu.DTOs;

namespace PizzaApp.BL.Features.Menu.Validators;

public class UpdateMenuItemModelValidator : AbstractValidator<UpdateMenuItemModel>
{
    public UpdateMenuItemModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
            .When(x => x.Name != null);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description cannot be empty.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => x.Description != null);

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("ImageUrl cannot be empty.")
            .MaximumLength(300).WithMessage("ImageUrl must not exceed 300 characters.")
            .When(x => x.ImageUrl != null);

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be positive.")
            .When(x => x.Price.HasValue);
    }
}