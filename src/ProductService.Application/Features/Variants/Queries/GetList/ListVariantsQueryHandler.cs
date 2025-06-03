using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Variants;
using SharedLibrary.Dtos.Variants;
using SharedLibrary.Filters;
using SharedLibrary.Wrappers;

namespace ProductService.Application.Features.Variants.Queries.GetList;

public class ListVariantsQueryHandler : IRequestHandler<ListVariantsQuery, PagedResponse<List<VariantDto>>>
{
    private readonly IMapper _mapper;
    private readonly IVariantReadOnlyRepository _readOnlyrepository;
    public ListVariantsQueryHandler(IMapper mapper,
                                      IVariantReadOnlyRepository readOnlyrepository)
    {
        _mapper = mapper;
        _readOnlyrepository = readOnlyrepository;
    }
    public async Task<PagedResponse<List<VariantDto>>> Handle(ListVariantsQuery query, CancellationToken cancellationToken)
    {
        var response = await PagedResponse(query);
        var result = _mapper.Map<PagedResponse<List<VariantDto>>>(response);

        return result;
    }
    private async Task<PagedResponse<List<Variant>>> PagedResponse(ListVariantsQuery query)
    {
        var queryable = _readOnlyrepository.GetQueryable();
        var pageFilter = query.Filter;
        var productId = query.ProductId;
        if (productId != Guid.Empty)
        {
            queryable = queryable.Where(x => x.ProductId == productId);
        }
        var validFilter = new PaginationFilter(pageFilter.PageNumber, pageFilter.PageSize);
        var pagedData = await queryable.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                                       .Take(validFilter.PageSize)
                                       .ToListAsync();
        var totalRecords = await queryable.CountAsync();
        var totalPages = ((double)totalRecords / validFilter.PageSize);
        var response = new PagedResponse<List<Variant>>(pagedData,
                                                        validFilter.PageNumber,
                                                        validFilter.PageSize)
        {
            TotalPages = Convert.ToInt32(Math.Ceiling(totalPages)),
            TotalRecords = totalRecords
        };

        return response;
    }
}
