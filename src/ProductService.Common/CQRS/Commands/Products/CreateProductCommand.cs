using MediatR;
using ProductService.Common.Dtos.Products;

namespace ProductService.Common.CQRS.Commands.Products;

public record CreateProductCommand(CreateProductDto model) : IRequest<Guid>;