using AutoMapper;
using MediatR;
using ProductService.Common.CQRS.UseCases.Products.CreateProduct;
using ProductService.Common.Dtos.Products;
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
        var model = new ProductDto(Guid.NewGuid(),
                                   command.model.Name,
                                   command.model.Code,
                                   command.model.Note,
                                   command.model.CostPrice,
                                   command.model.UnitPrice,
                                   command.model.Images,
                                   Guid.Empty,
                                   DateTime.UtcNow,
                                   null,
                                   null,
                                   null);
        var product = _mapper.Map<Product>(model);
        await _repository.AddAsync(product);
        return product.Id;
    }
}
