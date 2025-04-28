using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Authorize;
using ProductService.Application.Features.Variants.Commands.Create;
using ProductService.Application.Features.Variants.Queries.GetList;
using SharedLibrary.Constants.Permission;
using SharedLibrary.Dtos.Variants;

namespace ProductService.Api.Controllers.v1;

[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class VariantController : ControllerBase
{
    private readonly ISender _sender;
    public VariantController(ISender sender)
    {
        _sender = sender;
    }

    [PermissionAuthorize(Permissions.Products.View)]
    [HttpGet("{productId:guid}")]
    public async Task<IActionResult> GetListByProductId(Guid productId, [FromQuery] ListVariantsQuery model)
    {
        var query = new ListVariantsQuery(productId, model.Filter);
        var result = await _sender.Send(query);
        return Ok(result);
    }

    [PermissionAuthorize(Permissions.Products.Edit)]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVariantDto model)
    {
        var command = new CreateVariantCommand(model);
        var result = await _sender.Send(command);
        return Ok(result);
    }
}