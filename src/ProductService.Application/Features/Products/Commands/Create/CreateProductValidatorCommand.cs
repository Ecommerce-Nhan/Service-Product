using FluentValidation;

namespace ProductService.Application.Features.Products.Commands.Create;

public class CreateProductValidatorCommand : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidatorCommand()
    {
        RuleFor(x => x.Model.Name).NotEmpty();
        RuleFor(x => x.Model.Code).NotEmpty()
                                  .Length(6).WithMessage("Code must be 6 characters.");

    }
}