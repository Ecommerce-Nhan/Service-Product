using FluentValidation;

namespace ProductService.Application.Features.Categories.Queries.GetById;

public class GetCategoryValidatorQuery : AbstractValidator<GetCategoryQuery>
{
    public GetCategoryValidatorQuery()
    {
        RuleFor(x => x.Id).NotEmpty().NotNull().WithMessage("Id is required.");
    }
}

