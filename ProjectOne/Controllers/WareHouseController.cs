using Microsoft.AspNetCore.Mvc;
using ProjectOne.Application.WareHouse.CreateWareHouse;
using ProjectOne.Application.WareHouse.DeleteWareHouse;
using ProjectOne.Application.WareHouse.GetWareHouse;
using ProjectOne.Application.WareHouse.PatchWareHouse;
using ProjectOne.Application.WareHouse.UpdateWareHouse;
using ProjectOne.Extensions;

namespace ProjectOne.Controllers;

[ApiController]
[Route("[controller]")]
public class WareHouseController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> AddWareHouseAsync(
        [FromServices] CreateWareHouseHandler createWareHouseHandler,
        [FromBody] CreateWareHouseRequest createWareHouseRequest,
        CancellationToken cancellationToken = default)
    {
        var wareHouse = await createWareHouseHandler.CreateAsync(createWareHouseRequest, cancellationToken);

        return wareHouse.ToResponse();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WareHouseResponse>> GetWareHouseAsync(
        [FromServices] GetWareHouseHandler handler,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.GetAsync(id, cancellationToken);
        return result.ToResponse();
    }

    [HttpGet]
    public async Task<ActionResult<List<WareHouseResponse>>> GetWareHousesAsync(
        [FromServices] GetWareHousesHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.GetAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateWareHouseAsync(
        [FromServices] UpdateWareHouseHandler handler,
        Guid id,
        [FromBody] UpdateWareHouseRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.UpdateAsync(id, request, cancellationToken);
        return result.ToResponse();
    }

    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<Guid>> PatchWareHouseAsync(
        [FromServices] PatchWareHouseHandler handler,
        Guid id,
        [FromBody] PatchWareHouseRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.PatchAsync(id, request, cancellationToken);
        return result.ToResponse();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteWareHouseAsync(
        [FromServices] DeleteWareHouseHandler handler,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.Error.ToResponse();
    }
}