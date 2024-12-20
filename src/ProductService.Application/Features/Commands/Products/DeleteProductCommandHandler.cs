using MediatR;
using ProductService.Common.CQRS.UseCases.Products.DeleteProduct;
using ProductService.Common.Exceptions;
using ProductService.Domain.Products;

namespace ProductService.Application.Features.Commands.Products;

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
