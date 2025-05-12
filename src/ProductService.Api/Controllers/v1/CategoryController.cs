using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orchestration.ServiceDefaults.Authorize;
using ProductService.Application.Features.Categories.Queries.GetList;
using SharedLibrary.Constants.Permission;

namespace ProductService.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CategoryController : Controller
{
    private readonly ISender _sender;
    public CategoryController(ISender sender)
    {
        _sender = sender;
    }

    [PermissionAuthorize(Permissions.Categories.View)]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ListCategoriesQuery model)
    {
        var query = new ListCategoriesQuery(model.Pagination);
        var result = await _sender.Send(query);
        return Ok(result);
    }
}
