using MediatR;
using SharedLibrary.Dtos.Products;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;

namespace ProductService.Application.Features.Products.Queries.GetList;

public class ListProductsQuery : IRequest<PagedResponse<List<ProductDto>>>
{
    public PageRequest Pagination { get; set; } = new();
    public ListProductsQuery()
    {
    }
    public ListProductsQuery(PageRequest pagination)
    {
        Pagination = pagination;
    }
}