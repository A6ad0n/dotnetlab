using FluentValidation;
using PizzaApp.BL.Features.Discounts.DTOs;

namespace PizzaApp.BL.Features.Discounts.Validators;

public class CreateDiscountModelValidator : AbstractValidator<CreateDiscountModel>
{
    public CreateDiscountModelValidator()
    {
        RuleFor(x => x)
            .Must(x => x.StatusId.HasValue || x.StatusExternalId.HasValue)
            .WithMessage("Хотя бы одно из полей StatusId или StatusExternalId должно быть заполнено");
    }
}