using FluentValidation;

namespace ProductService.Common.CQRS.UseCases.Products.DeleteProduct;

public class DeleteProductValidatorCommand : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductValidatorCommand()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Id is required.");
    }
}
