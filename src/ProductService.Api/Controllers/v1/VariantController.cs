using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orchestration.ServiceDefaults.Authorize;
using ProductService.Application.Features.Variants.Commands.Create;
using ProductService.Application.Features.Variants.Commands.Delete;
using ProductService.Application.Features.Variants.Commands.Update;
using ProductService.Application.Features.Variants.Queries.GetList;
using ProductService.Application.Features.Variants.Queries.GetListByProductId;
using SharedLibrary.Constants.Permission;
using SharedLibrary.Dtos.Variants;
using SharedLibrary.Filters;

namespace ProductService.Api.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/product/[controller]")]
[ApiController]
public class VariantController : ControllerBase
{
    private readonly ISender _sender;
    public VariantController(ISender sender)
    {
        _sender = sender;
    }

    [PermissionAuthorize(Permissions.Products.View)]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ListVariantsQuery model)
    {
        var query = new ListVariantsQuery(model.Pagination);
        var result = await _sender.Send(query);
        return Ok(result);
    }

    [PermissionAuthorize(Permissions.Products.Edit)]
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateVariantDto model)
    {
        var command = new CreateVariantCommand(model);
        var result = await _sender.Send(command);
        return Ok(result);
    }

    [PermissionAuthorize(Permissions.Products.Edit)]
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] UpdateVariantDto model)
    {
        var command = new UpdateVariantCommand(id, model);
        await _sender.Send(command);
        return NoContent();
    }

    [PermissionAuthorize(Permissions.Products.Delete)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteVariantCommand(id);
        await _sender.Send(command);
        return NoContent();
    }

    [PermissionAuthorize(Permissions.Products.View)]
    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetByProductId(Guid productId, [FromQuery] PaginationFilter pagination)
    {
        var query = new ListVariantsByProductIdQuery(pagination, productId);
        var result = await _sender.Send(query);
        return Ok(result);
    }
}