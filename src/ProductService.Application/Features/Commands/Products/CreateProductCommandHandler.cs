using AutoMapper;
using MediatR;
using ProductService.Common.CQRS.UseCases.Products.CreateProduct;
using ProductService.Domain.Products;

namespace ProductService.Application.Features.Commands.Products;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _repository;
    private readonly ProductManager _manager;
    public CreateProductCommandHandler(IMapper mapper,
                             IProductRepository repository,
                             ProductManager manager)
    {
        _repository = repository;
        _mapper = mapper;
        _manager = manager;
    }
    public async Task<Guid> Handle(CreateProductCommand command,
                             CancellationToken cancellationToken)
    {
        //var product = _mapper.Map<Product>(command.Model);
        var input = command.Model;
        var product = await _manager.CreateAsync(
                input.Name,
                input.Code,
                input.Note,
                input.UnitPrice,
                input.CostPrice,
                input.Images
            );
        await _repository.AddAsync(product);
        return product.Id;
    }
}
