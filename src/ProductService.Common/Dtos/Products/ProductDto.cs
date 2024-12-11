using System.Collections.Immutable;

namespace ProductService.Common.Dtos.Products;

public record ProductDto(Guid Id,
                         string Name,
                         string Code,
                         string? Note,
                         float CostPrice,
                         float UnitPrice,
                         ImmutableList<string>? Images,
                         Guid CreateBy,
                         DateTime CreateAt,
                         Guid? UpdateBy,
                         DateTime? UpdateAt,
                         DateTime? DeletedAt);