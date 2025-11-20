using MediatR;

namespace ProductService.Application.Features.Variants.Commands.Delete;

public record DeleteVariantCommand(Guid Id) : IRequest;

