using MediatR;
using SharedLibrary.Dtos.Variants;

namespace ProductService.Application.Features.Variants.Commands.Create;

public record CreateVariantCommand(CreateVariantDto Model) : IRequest<Guid>;