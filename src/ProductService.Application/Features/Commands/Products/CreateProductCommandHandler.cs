using AutoMapper;
using MediatR;
using ProductService.Common.CQRS.UseCases.Products.CreateProduct;
using ProductService.Domain.Products;

namespace ProductService.Application.Features.Commands.Products;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repository;
    public CreateProductCommandHandler(IMapper mapper,
                             IProductRepository repository)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<Guid> Handle(CreateProductCommand command,
                             CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Product>(command.Model);
        await _repository.AddAsync(product);
        return product.Id;
    }
}
