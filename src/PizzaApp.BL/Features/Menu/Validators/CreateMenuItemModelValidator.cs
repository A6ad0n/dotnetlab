using Duende.IdentityServer.Extensions;
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
        
        RuleFor(x => x)
            .Must(x => x.CategoryId.HasValue ^ x.CategoryExternalId.HasValue)
            .WithMessage("Только одно из полей CategoryId или CategoryExternalId должно быть заполнено");

        RuleFor(x => x.StatusId)
            .GreaterThan(0).WithMessage("StatusId must be a positive integer.");
        
        RuleFor(x => x)
            .Must(x => x.StatusId.HasValue ^ x.StatusExternalId.HasValue)
            .WithMessage("Only one of the fields StatusId or StatusExternalId must be filled.");
        
        RuleFor(x => x)
            .Must(x => (!x.DiscountIds.IsNullOrEmpty() && x.DiscountExternalIds.IsNullOrEmpty()) ||
                       (x.DiscountIds.IsNullOrEmpty() && !x.DiscountExternalIds.IsNullOrEmpty()) ||
                       (x.DiscountIds.IsNullOrEmpty() && x.DiscountExternalIds.IsNullOrEmpty()))
            .WithMessage("Only one of the fields DiscountIds or DiscountExternalIds must be filled.");
    }
}
