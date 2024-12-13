using AutoMapper;
using ProductService.Common.Dtos.Products;
using ProductService.Domain.Products;
using System.Collections.Immutable;

namespace ProductService.Application.Mappers;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductDto, Product>();
        CreateMap<Product, ProductDto>();
    }
}
