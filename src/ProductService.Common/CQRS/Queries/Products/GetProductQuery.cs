using MediatR;
using ProductService.Common.Dtos.Products;

namespace ProductService.Common.CQRS.Queries.Product;

public record GetProductQuery(Guid Id) : IRequest<ProductDto>;