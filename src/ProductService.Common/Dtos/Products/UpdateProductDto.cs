namespace ProductService.Common.Dtos.Products;

public record UpdateProductDto(Guid Id,
                               string Name,
                               string Code,
                               string? Note,
                               float CostPrice,
                               float UnitPrice,
                               List<string>? Images);
