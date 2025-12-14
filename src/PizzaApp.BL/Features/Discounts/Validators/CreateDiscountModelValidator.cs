using FluentValidation;
using PizzaApp.BL.Features.Discounts.DTOs;

namespace PizzaApp.BL.Features.Discounts.Validators;

public class CreateDiscountModelValidator : AbstractValidator<CreateDiscountModel>
{
    public CreateDiscountModelValidator()
    {
        RuleFor(x => x)
            .Must(x => x.StatusId.HasValue ^ x.StatusExternalId.HasValue)
            .WithMessage("Only one of the fields StatusId or StatusExternalId must be filled.");
    }
}