using FluentValidation;

namespace ProductService.Application.Features.Variants.Commands.Update;

public class UpdateVariantValidatorCommand : AbstractValidator<UpdateVariantCommand>
{
    public UpdateVariantValidatorCommand()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Id is required.");
        RuleFor(x => x.Model.ProductId).NotEmpty().WithMessage("Product Id is required.");
        RuleFor(x => x.Model.SKU).NotEmpty().WithMessage("Code SKU is required.");
        RuleFor(x => x.Model.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        RuleFor(x => x.Model.UnitPrice).GreaterThan(0).WithMessage("Unit Price must be greater than 0.");
        RuleFor(x => x.Model.MainImage).NotEmpty().WithMessage("Main Image is required.");
        RuleFor(x => x.Model.Attributes)
            .NotEmpty().WithMessage("Attributes cannot be empty.");
    }
}

