using AutoMapper;
using ProductService.Common.Wrappers;

namespace ProductService.Application.Mappers;

public class ApplicationAutoMapperProfile : Profile
{
    public ApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap(typeof(PagedResponse<>), typeof(PagedResponse<>))
           .ConvertUsing(typeof(PagedResponseConverter<,>));
    }
}
public class PagedResponseConverter<TSource, TDestination> : ITypeConverter<PagedResponse<TSource>, PagedResponse<TDestination>>
{
    public PagedResponse<TDestination> Convert(PagedResponse<TSource> source, PagedResponse<TDestination> destination, ResolutionContext context)
    {
        var mappedData = context.Mapper.Map<TDestination>(source.Data);
        return new PagedResponse<TDestination>(mappedData, source.PageNumber, source.PageSize);
    }
}