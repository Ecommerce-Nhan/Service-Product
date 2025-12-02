using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orchestration.ServiceDefaults.Authorize;
using ProductService.Application.Features.Categories.Commands.Create;
using ProductService.Application.Features.Categories.Commands.Delete;
using ProductService.Application.Features.Categories.Commands.Update;
using ProductService.Application.Features.Categories.Queries.GetById;
using ProductService.Application.Features.Categories.Queries.GetList;
using SharedLibrary.Constants.Permission;
using SharedLibrary.Dtos.Categories;
using SharedLibrary.Filters;

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

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PageRequest pagination)
    {
        var query = new ListCategoriesQuery(pagination);
        var result = await _sender.Send(query);
        return Ok(result);
    }

    [PermissionAuthorize(Permissions.Categories.Edit)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDto model)
    {
        var command = new CreateCategoryCommand(model);
        var result = await _sender.Send(command);
        return Ok(result);
    }

    [PermissionAuthorize(Permissions.Categories.Edit)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateCategoryDto model)
    {
        var command = new UpdateCategoryCommand(id, model);
        await _sender.Send(command);
        return NoContent();
    }

    [PermissionAuthorize(Permissions.Categories.Delete)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteCategoryCommand(id);
        await _sender.Send(command);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetCategoryQuery(id);
        var result = await _sender.Send(query);
        return Ok(result);
    }
}
