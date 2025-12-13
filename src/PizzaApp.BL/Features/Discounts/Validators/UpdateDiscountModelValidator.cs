using FluentValidation;
using PizzaApp.BL.Features.Discounts.DTOs;

namespace PizzaApp.BL.Features.Discounts.Validators;

public class UpdateDiscountModelValidator : AbstractValidator<UpdateDiscountModel>
{
    public UpdateDiscountModelValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.")
            .When(x => x.Name != null);

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.")
            .When(x => x.Description != null);

        RuleFor(x => x.DiscountPercentage)
            .InclusiveBetween(0.01m, 100m).WithMessage("DiscountPercentage must be between 0.01 and 100.")
            .When(x => x.DiscountPercentage.HasValue);

        RuleFor(x => x.ValidFrom)
            .LessThan(x => x.ValidTo).WithMessage("ValidFrom must be earlier than ValidTo.")
            .When(x => x.ValidFrom.HasValue && x.ValidTo.HasValue);

        RuleFor(x => x.ValidTo)
            .GreaterThan(x => x.ValidFrom).WithMessage("ValidTo must be later than ValidFrom.")
            .When(x => x.ValidFrom.HasValue && x.ValidTo.HasValue);
    }
}