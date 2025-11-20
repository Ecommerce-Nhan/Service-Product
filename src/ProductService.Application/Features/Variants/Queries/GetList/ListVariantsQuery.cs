using MediatR;
using SharedLibrary.Dtos.Variants;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;

namespace ProductService.Application.Features.Variants.Queries.GetList;

public class ListVariantsQuery : IRequest<PagedResponse<List<VariantDto>>>
{
    public PaginationFilter Pagination { get; set; } = new();
    public ListVariantsQuery()
    {
    }
    public ListVariantsQuery(PaginationFilter pagination)
    {
        Pagination = pagination;
    }
}