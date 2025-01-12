using MediatR;
using AutoMapper;
using SharedLibrary.Dtos.Products;
using ProductService.Domain.Products;
using ProductService.Domain.Exceptions.Products;

namespace ProductService.Application.Features.Products.Queries.GetById;

public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDto>
{
    private readonly IMapper _mapper;
    private readonly IProductReadOnlyRepository _repository;

    public GetProductQueryHandler(IMapper mapper, IProductReadOnlyRepository repository)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ProductDto> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _repository.GetByIdAsync(request.Id);
        if (product == null)
        {
            throw new ProductNotFoundException(request.Id);
        }
        var result = _mapper.Map<ProductDto>(product);
        return result;
    }
}
