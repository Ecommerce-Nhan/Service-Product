using System.Collections.Immutable;

namespace ProductService.Common.Dtos.Products;

public record CreateProductDto(string Name,
                         string Code,
                         string? Note,
                         float CostPrice,
                         float UnitPrice,
                         ImmutableList<string>? Images);