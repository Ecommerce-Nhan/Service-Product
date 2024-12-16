
namespace ProductService.Common.Dtos.Products;

public record ProductDto(Guid Id,
                         string Name,
                         string Code,
                         string? Note,
                         float CostPrice,
                         float UnitPrice,
                         List<string>? Images);