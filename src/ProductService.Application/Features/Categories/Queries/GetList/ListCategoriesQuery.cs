using MediatR;
using SharedLibrary.Dtos.Categories;
using SharedLibrary.Dtos.Products;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;

namespace ProductService.Application.Features.Categories.Queries.GetList;

public class ListCategoriesQuery : IRequest<PagedResponse<List<CategoryDto>>>
{
    public PageRequest Pagination { get; set; } = new();
    public ListCategoriesQuery()
    {
    }
    public ListCategoriesQuery(PageRequest pagination)
    {
        Pagination = pagination;
    }
}