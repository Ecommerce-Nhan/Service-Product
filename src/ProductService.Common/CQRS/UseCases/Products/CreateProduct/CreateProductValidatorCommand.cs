using FluentValidation;

namespace ProductService.Common.CQRS.UseCases.Products.CreateProduct;

public class CreateProductValidatorCommand : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidatorCommand()
    {
        RuleFor(x => x.model.Name).NotEmpty().WithMessage("Name is required.")
                                  .Length(5, 50).WithMessage("Name must be between 5 and 50 characters.");

        RuleFor(x => x.model.CostPrice).InclusiveBetween(10, 500).WithMessage("Cost price must be between 10 and 500.");
    }
}