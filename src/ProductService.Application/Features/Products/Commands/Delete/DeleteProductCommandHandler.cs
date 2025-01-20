using MediatR;
using ProductService.Domain.Exceptions.Products;
using ProductService.Domain.Products;

namespace ProductService.Application.Features.Products.Commands.Delete;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductRepository _repository;
    public DeleteProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }
    public async Task Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        var product = _repository.GetQueryable().FirstOrDefault(x => x.Id == command.Id);
        if (product == null)
        {
            throw new ProductNotFoundException(command.Id);
        }
        await _repository.Remove(product);
    }
}
