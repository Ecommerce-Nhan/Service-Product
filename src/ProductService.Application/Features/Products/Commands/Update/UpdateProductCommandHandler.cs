using MediatR;
using ProductService.Domain.Exceptions.Products;
using ProductService.Domain.Products;

namespace ProductService.Application.Features.Products.Commands.Update;

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
        //product.Images = input.Images;

        await _repository.Update(product);
    }
}
