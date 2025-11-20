using FluentValidation;

namespace ProductService.Application.Features.Variants.Commands.Create;

public class CreateVariantValidatorCommand : AbstractValidator<CreateVariantCommand>
{
    public CreateVariantValidatorCommand()
    {
        RuleFor(x => x.Model.ProductId).NotEmpty().WithMessage("Product Id is required.");
        RuleFor(x => x.Model.SKU).NotEmpty().WithMessage("Code SKU is required.");
        RuleFor(x => x.Model.Quantity).GreaterThan(0);
        RuleFor(x => x.Model.UnitPrice).GreaterThan(0);
        RuleFor(x => x.Model.MainImage).NotEmpty();
        RuleFor(x => x.Model.Attributes)
            .NotEmpty().WithMessage("Attributes cannot be empty.");
    }
}