using FluentValidation;

namespace ProductService.Application.Features.Categories.Commands.Delete;

public class DeleteCategoryValidatorCommand : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryValidatorCommand()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Id is required.");
    }
}

