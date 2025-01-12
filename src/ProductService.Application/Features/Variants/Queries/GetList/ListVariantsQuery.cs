using MediatR;
using SharedLibrary.Dtos.Variants;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;

namespace ProductService.Application.Features.Variants.Queries.GetList;

public class ListVariantsQuery : IRequest<PagedResponse<List<VariantDto>>>
{
    public Guid ProductId { get; set; }
    public PaginationFilter Filter { get; set; } = new();
    public ListVariantsQuery()
    {
    }
    public ListVariantsQuery(Guid productId,
                               PaginationFilter filter)
    {
        ProductId = productId;
        Filter = filter;
    }
}
