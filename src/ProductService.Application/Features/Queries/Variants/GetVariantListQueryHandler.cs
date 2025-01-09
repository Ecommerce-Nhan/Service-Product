using AutoMapper;
using MediatR;
using ProductService.Domain.Variants;
using SharedLibrary.CQRS.UseCases.Variants.GetVariantByProductId;
using SharedLibrary.Dtos.Variants;
using SharedLibrary.Wrappers;

namespace VariantService.Application.Features.Queries.Variants;

public class GetVariantListQueryHandler : IRequestHandler<GetVariantListQuery, PagedResponse<List<VariantDto>>>
{
    private readonly IMapper _mapper;
    private readonly IVariantReadOnlyRepository _readOnlyrepository;
    public GetVariantListQueryHandler(IMapper mapper,
                                      IVariantReadOnlyRepository readOnlyrepository)
    {
        _mapper = mapper;
        _readOnlyrepository = readOnlyrepository;
    }
    public async Task<PagedResponse<List<VariantDto>>> Handle(GetVariantListQuery query, CancellationToken cancellationToken)
    {
        var pagedData = await _readOnlyrepository.GetPageAsync(query);
        var result = _mapper.Map<PagedResponse<List<VariantDto>>>(pagedData);
        return result;
    }
}
