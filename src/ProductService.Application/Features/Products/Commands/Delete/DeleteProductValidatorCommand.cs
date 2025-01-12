using FluentValidation;

namespace ProductService.Application.Features.Products.Commands.Delete;

public class DeleteProductValidatorCommand : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidatorCommand()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Id is required.");
    }
}