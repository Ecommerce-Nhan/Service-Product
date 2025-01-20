using AutoMapper;
using MediatR;
using ProductService.Domain.Products;
using SharedLibrary.Dtos.Products;
using SharedLibrary.Wrappers;

namespace ProductService.Application.Features.Products.Queries.GetList;

public class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, PagedResponse<List<ProductDto>>>
{
    private readonly IMapper _mapper;
    private readonly IProductReadOnlyRepository _readOnlyrepository;
    public ListProductsQueryHandler(IMapper mapper,
                                      IProductReadOnlyRepository readOnlyrepository)
    {
        _mapper = mapper;
        _readOnlyrepository = readOnlyrepository;
    }
    public async Task<PagedResponse<List<ProductDto>>> Handle(ListProductsQuery query, CancellationToken cancellationToken)
    {
        var pagedData = await _readOnlyrepository.GetPageAsync(query.Filter);
        var result = _mapper.Map<PagedResponse<List<ProductDto>>>(pagedData);
        return result;
    }
}
