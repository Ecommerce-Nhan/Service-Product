using MediatR;
using ProductService.Domain.Exceptions.Variants;
using ProductService.Domain.Variants;
using System.Text.Json;

namespace ProductService.Application.Features.Variants.Commands.Update;

public class UpdateVariantCommandHandler : IRequestHandler<UpdateVariantCommand>
{
    private readonly IVariantRepository _repository;
    private readonly VariantManager _manager;
    public UpdateVariantCommandHandler(IVariantRepository repository,
                                       VariantManager manager)
    {
        _repository = repository;
        _manager = manager;
    }
    public async Task Handle(UpdateVariantCommand command,
                             CancellationToken cancellationToken)
    {
        var input = command.Model;
        var variant = _repository.GetQueryable().FirstOrDefault(x => x.Id == command.Id);
        if (variant == null)
        {
            throw new VariantNotFoundException(command.Id);
        }
        if (variant.SKU != input.SKU)
        {
            await _manager.ChangeSKUAsync(variant, input.SKU);
        }
        variant.UnitPrice = input.UnitPrice;
        variant.Quantity = input.Quantity;
        variant.MainImage = input.MainImage;
        variant.Attributes = JsonSerializer.Serialize(input.Attributes);

        await _repository.Update(variant);
    }
}

