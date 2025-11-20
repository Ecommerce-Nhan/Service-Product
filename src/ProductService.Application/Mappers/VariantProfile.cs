using AutoMapper;
using ProductService.Domain.Variants;
using SharedLibrary.Dtos.Variants;

namespace ProductService.Application.Mappers;

public class VariantProfile : Profile
{
    public VariantProfile()
    {
        CreateMap<Variant, VariantDto>();
    }
}