using FluentValidation;
using PizzaApp.BL.Features.Menu.DTOs;

namespace PizzaApp.BL.Features.Menu.Validators;

public class CreateMenuItemModelValidator : AbstractValidator<CreateMenuItemModel>
{
    public CreateMenuItemModelValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.ImageUrl)
            .NotEmpty().WithMessage("ImageUrl is required.")
            .MaximumLength(300).WithMessage("ImageUrl must not exceed 300 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("CategoryId must be a positive integer.");

        RuleFor(x => x.StatusId)
            .GreaterThan(0).WithMessage("StatusId must be a positive integer.");
    }
}
