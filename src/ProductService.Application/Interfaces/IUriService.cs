using ProductService.Common.Filters;

namespace ProductService.Application.Interfaces;

public interface IUriService
{
    public Uri GetPageUri(PaginationFilter filter, string route);
}
