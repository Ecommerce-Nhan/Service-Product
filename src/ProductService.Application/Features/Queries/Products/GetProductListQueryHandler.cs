using AutoMapper;
using MediatR;
using ProductService.Common.CQRS.UseCases.Products.GetListProducts;
using ProductService.Common.Dtos.Products;
using ProductService.Common.Wrappers;
using ProductService.Domain.Products;

namespace ProductService.Application.Features.Queries.Products;

public class GetProductListQueryHandler : IRequestHandler<GetProductListQuery, PagedResponse<List<ProductDto>>>
{
    private readonly IMapper _mapper;
    private readonly IProductReadOnlyRepository _readOnlyrepository;
    public GetProductListQueryHandler(IMapper mapper,
                                      IProductReadOnlyRepository readOnlyrepository)
    {
        _mapper = mapper;
        _readOnlyrepository = readOnlyrepository;
    }
    public async Task<PagedResponse<List<ProductDto>>> Handle(GetProductListQuery query, CancellationToken cancellationToken)
    {
        var pagedData = await _readOnlyrepository.GetPageAsync(query.Filter);
        var result = _mapper.Map<PagedResponse<List<ProductDto>>>(pagedData);
        return result;
    }
}
