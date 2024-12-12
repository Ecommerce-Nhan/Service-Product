using MediatR;
using ProductService.Common.Dtos.Products;

namespace ProductService.Common.CQRS.UseCases.Products.GetProductById;

public record GetProductQuery(Guid Id) : IRequest<ProductDto>;