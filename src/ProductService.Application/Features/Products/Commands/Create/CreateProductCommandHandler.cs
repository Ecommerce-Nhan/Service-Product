using MediatR;
using ProductService.Application.Features.Products.Notifications;
using ProductService.Domain.Products;

namespace ProductService.Application.Features.Products.Commands.Create;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IMediator _mediatR;
    private readonly IProductRepository _repository;
    private readonly ProductManager _manager;
    public CreateProductCommandHandler(IProductRepository repository,
                                       ProductManager manager,
                                       IMediator mediatR)
    {
        _repository = repository;
        _manager = manager;
        _mediatR = mediatR;
    }
    public async Task<Guid> Handle(CreateProductCommand command,
                                   CancellationToken cancellationToken)
    {
        var input = command.Model;
        var product = await _manager.CreateAsync(
                                input.CategoryId,
                                input.Name,
                                input.Code,
                                input.Note,
                                input.CostPrice
                            );

        await _mediatR.Publish(new ProductCreatedNotification(input, product));
        await _repository.AddAsync(product);
        return product.Id;
    }
}