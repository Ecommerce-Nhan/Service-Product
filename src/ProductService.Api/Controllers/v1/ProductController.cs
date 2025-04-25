using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Authorize;
using ProductService.Application.Features.Products.Commands.Create;
using ProductService.Application.Features.Products.Commands.Delete;
using ProductService.Application.Features.Products.Commands.Update;
using ProductService.Application.Features.Products.Queries.GetById;
using ProductService.Application.Features.Products.Queries.GetList;
using SharedLibrary.Constants.Permission;
using SharedLibrary.Dtos.Products;

namespace ProductService.Api.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ISender _sender;
    public ProductController(ISender sender)
    {
        _sender = sender;
    }

    [PermissionAuthorize(Permissions.Products.View)]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ListProductsQuery model)
    {
        var query = new ListProductsQuery(model.Pagination);
        var result = await _sender.Send(query);
        return Ok(result);
    }

    [PermissionAuthorize(Permissions.Products.Create)]
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] CreateProductDto model)
    {
        var command = new CreateProductCommand(model);
        var result = await _sender.Send(command);
        return Ok(result);
    }

    [PermissionAuthorize(Permissions.Products.Edit)]
    [HttpPut]
    public async Task<IActionResult> Put([FromForm] UpdateProductDto model)
    {
        var command = new UpdateProductCommand(model);
        await _sender.Send(command);
        return NoContent();
    }

    [PermissionAuthorize(Permissions.Products.Delete)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteProductCommand(id);
        await _sender.Send(command);
        return NoContent();
    }

    [PermissionAuthorize(Permissions.Products.View)]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var query = new GetProductQuery(id);
        var result = await _sender.Send(query);
        return Ok(result);
    }
}