using FluentValidation;

namespace ProductService.Application.Features.Products.Queries.GetById;

public class GetProductValidatorQuery : AbstractValidator<GetProductQuery>
{
    public GetProductValidatorQuery()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Id is required.");
    }
}
