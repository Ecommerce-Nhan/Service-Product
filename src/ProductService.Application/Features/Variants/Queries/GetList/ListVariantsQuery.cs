using MediatR;
using SharedLibrary.Dtos.Variants;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;

namespace ProductService.Application.Features.Variants.Queries.GetList;

public class ListVariantsQuery : IRequest<PagedResponse<List<VariantDto>>>
{
    public PageRequest Pagination { get; set; } = new();
    public ListVariantsQuery()
    {
    }
    public ListVariantsQuery(PageRequest pagination)
    {
        Pagination = pagination;
    }
}