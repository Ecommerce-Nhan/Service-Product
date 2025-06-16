using Asp.Versioning;
using CategoryService.Application.Features.Categories.Commands.Create;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orchestration.ServiceDefaults.Authorize;
using ProductService.Application.Features.Categories.Queries.GetList;
using SharedLibrary.Constants.Permission;
using SharedLibrary.Dtos.Categories;

namespace ProductService.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/product/[controller]")]
public class CategoryController : ControllerBase
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

    //[PermissionAuthorize(Permissions.Categories.View)]
    [HttpPost]
    public async Task<IActionResult> Create([FromQuery] CreateCategoryDto model)
    {
        var command = new CreateCategoryCommand(model);
        var result = await _sender.Send(command);
        return Ok(result);
    }
}
