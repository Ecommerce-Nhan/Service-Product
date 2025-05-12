using MediatR;
using SharedLibrary.Dtos.Categories;
using SharedLibrary.Dtos.Products;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;

namespace ProductService.Application.Features.Categories.Queries.GetList;

public class ListCategoriesQuery : IRequest<PagedResponse<List<CategoryDto>>>
{
    public PaginationFilter Pagination { get; set; } = new();
    public ListCategoriesQuery()
    {
    }
    public ListCategoriesQuery(PaginationFilter pagination)
    {
        Pagination = pagination;
    }
}