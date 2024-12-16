using FluentValidation;

namespace ProductService.Common.CQRS.UseCases.Products.GetProductById;

public class GetProductValidatorQuery : AbstractValidator<GetProductQuery>
{
    public GetProductValidatorQuery()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Id is required.");
    }
}
