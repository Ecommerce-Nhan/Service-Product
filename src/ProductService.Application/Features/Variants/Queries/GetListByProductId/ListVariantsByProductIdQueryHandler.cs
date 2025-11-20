using AutoMapper;
using MediatR;
using ProductService.Domain.Variants;
using SharedLibrary.Dtos.Variants;
using SharedLibrary.Wrappers;

namespace ProductService.Application.Features.Variants.Queries.GetListByProductId;

public class ListVariantsByProductIdQueryHandler
    : IRequestHandler<ListVariantsByProductIdQuery, PagedResponse<List<VariantDto>>>
{
    private readonly IMapper _mapper;
    private readonly IVariantReadOnlyRepository _readOnlyrepository;
    public ListVariantsByProductIdQueryHandler(IMapper mapper,
                                    IVariantReadOnlyRepository readOnlyrepository)
    {
        _mapper = mapper;
        _readOnlyrepository = readOnlyrepository;
    }
    public async Task<PagedResponse<List<VariantDto>>> Handle(ListVariantsByProductIdQuery query, CancellationToken cancellationToken)
    {
        var pagedData = await _readOnlyrepository.GetPageByProductIdAsync(query.ProductId, query.Pagination);
        var result = _mapper.Map<PagedResponse<List<VariantDto>>>(pagedData);
        return result;
    }
}