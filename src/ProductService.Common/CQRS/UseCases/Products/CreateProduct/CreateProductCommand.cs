using MediatR;
using ProductService.Common.Dtos.Products;

namespace ProductService.Common.CQRS.UseCases.Products.CreateProduct;

public record CreateProductCommand(CreateProductDto model) : IRequest<Guid>;