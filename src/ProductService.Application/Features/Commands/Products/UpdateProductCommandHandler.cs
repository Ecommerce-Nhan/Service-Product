using MediatR;
using SharedLibrary.CQRS.UseCases.Products.UpdateProduct;
using ProductService.Domain.Products;
using SharedLibrary.Exceptions.Products;

namespace ProductService.Application.Features.Commands.Products;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _repository;
    private readonly ProductManager _manager;
    public UpdateProductCommandHandler(IProductRepository repository,
                                       ProductManager manager)
    {
        _repository = repository;
        _manager = manager;
    }
    public async Task Handle(UpdateProductCommand command,
                             CancellationToken cancellationToken)
    {
        var input = command.Model;
        var product = _repository.GetQueryable().FirstOrDefault(x => x.Id == input.Id);
        if (product == null)
        {
            throw new ProductNotFoundException(input.Id);
        }
        if (product.Code != input.Code)
        {
            await _manager.ChangeCodeAsync(product, input.Code);
        }

        if (product.Name != input.Name)
        {
            await _manager.ChangeNameAsync(product, input.Name);
        }
        product.Note = input.Note;
        product.CostPrice = input.CostPrice;
        product.Images = input.Images;
        
        await _repository.Update(product);
    }
}
