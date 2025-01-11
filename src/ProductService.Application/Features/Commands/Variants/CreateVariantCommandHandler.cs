using MediatR;
using ProductService.Domain.Variants;
using SharedLibrary.CQRS.UseCases.Variants.CreateVariant;

namespace VariantService.Application.Features.Commands.Variants;

public class CreateVariantCommandHandler : IRequestHandler<CreateVariantCommand, Guid>
{
    private readonly IVariantRepository _repository;
    private readonly VariantManager _manager;
    public CreateVariantCommandHandler(IVariantRepository repository,
                                       VariantManager manager)
    {
        _repository = repository;
        _manager = manager;
    }
    public async Task<Guid> Handle(CreateVariantCommand command,
                             CancellationToken cancellationToken)
    {
        var input = command.Model;
        var variant = await _manager.CreateAsync(
                input.ProductId,
                input.SKU,
                input.UnitPrice,
                input.Quantity,
                input.MainImage,
                input.Attributes
            );
        await _repository.AddAsync(variant);

        return variant.Id;
    }
}
