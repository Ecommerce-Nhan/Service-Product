using MediatR;
using ProductService.Domain.Exceptions.Variants;
using ProductService.Domain.Variants;

namespace ProductService.Application.Features.Variants.Commands.Delete;

public class DeleteVariantCommandHandler : IRequestHandler<DeleteVariantCommand>
{
    private readonly IVariantRepository _repository;
    public DeleteVariantCommandHandler(IVariantRepository repository)
    {
        _repository = repository;
    }
    public async Task Handle(DeleteVariantCommand command, CancellationToken cancellationToken)
    {
        var variant = _repository.GetQueryable().FirstOrDefault(x => x.Id == command.Id);
        if (variant == null)
        {
            throw new VariantNotFoundException(command.Id);
        }
        await _repository.Remove(variant);
    }
}

