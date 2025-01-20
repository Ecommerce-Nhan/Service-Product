using MediatR;
using SharedLibrary.Dtos.Products;

namespace ProductService.Application.Features.Products.Commands.Create;

public record CreateProductCommand(CreateProductDto Model) : IRequest<Guid>;