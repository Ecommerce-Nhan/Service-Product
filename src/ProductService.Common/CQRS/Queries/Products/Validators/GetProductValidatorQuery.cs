using FluentValidation;
using ProductService.Common.CQRS.Queries.Product;

namespace ProductService.Common.CQRS.Queries.Products.Validators;

public class GetProductValidatorQuery : AbstractValidator<GetProductQuery>
{
    public GetProductValidatorQuery()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull();
    }
}
