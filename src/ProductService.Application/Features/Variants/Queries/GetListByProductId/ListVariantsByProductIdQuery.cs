using MediatR;
using SharedLibrary.Dtos.Variants;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;

namespace ProductService.Application.Features.Variants.Queries.GetListByProductId;

public class ListVariantsByProductIdQuery : IRequest<PagedResponse<List<VariantDto>>>
{
    public PaginationFilter Pagination { get; set; } = new();
    public Guid ProductId { get; set; }
    public ListVariantsByProductIdQuery()
    {
    }
    public ListVariantsByProductIdQuery(PaginationFilter pagination, Guid productId)
    {
        Pagination = pagination;
        ProductId = productId;
    }
}