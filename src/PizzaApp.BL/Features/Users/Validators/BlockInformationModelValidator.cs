using FluentValidation;
using PizzaApp.BL.Features.Users.Entities;

namespace PizzaApp.BL.Features.Users.Validators;

public class BlockInformationModelValidator : AbstractValidator<BlockInformationModel>
{
    public BlockInformationModelValidator()
    {
        RuleFor(x => x.BlockInformation)
            .Must((request, blockInfo) => request.IsBlocked != false || blockInfo == null)
            .WithMessage("Если пользователь не заблокирован, информация о блокировке должна быть null.");
    }
}