using FluentValidation;

namespace ProductService.Application.Features.Products.Commands.Update;

public class UpdateProductValidatorCommand : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidatorCommand()
    {
        RuleFor(x => x.Model.Name).NotEmpty().WithMessage("Name is required.")
                                  .Length(5, 50).WithMessage("Name must be between 5 and 50 characters.");

        RuleFor(x => x.Model.CostPrice).InclusiveBetween(10, 500).WithMessage("Cost price must be between 10 and 500.");
    }
}