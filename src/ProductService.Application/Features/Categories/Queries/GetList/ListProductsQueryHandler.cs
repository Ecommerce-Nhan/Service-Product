using AutoMapper;
using MediatR;
using SharedLibrary.Wrappers;
using ProductService.Application.Features.Categories.Queries.GetList;
using CategoryService.Domain.Categories;
using SharedLibrary.Dtos.Categories;

namespace CategoryService.Application.Features.Categories.Queries.GetList;

public class ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQuery, PagedResponse<List<CategoryDto>>>
{
    private readonly IMapper _mapper;
    private readonly ICategoryReadOnlyRepository _readOnlyrepository;
    public ListCategoriesQueryHandler(IMapper mapper,
                                      ICategoryReadOnlyRepository readOnlyrepository)
    {
        _mapper = mapper;
        _readOnlyrepository = readOnlyrepository;
    }
    public async Task<PagedResponse<List<CategoryDto>>> Handle(ListCategoriesQuery query, CancellationToken cancellationToken)
    {
        var pagedData = await _readOnlyrepository.GetPageAsync(query.Pagination);
        var result = _mapper.Map<PagedResponse<List<CategoryDto>>>(pagedData);
        return result;
    }
}
