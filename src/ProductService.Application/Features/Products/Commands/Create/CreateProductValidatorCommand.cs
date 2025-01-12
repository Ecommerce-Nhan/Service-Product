using FluentValidation;

namespace ProductService.Application.Features.Products.Commands.Create;

public class CreateProductValidatorCommand : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidatorCommand()
    {
        RuleFor(x => x.Model.Name).NotEmpty().WithMessage("Name is required.")
                                  .Length(5, 50).WithMessage("Name must be between 5 and 50 characters.");

    }
}