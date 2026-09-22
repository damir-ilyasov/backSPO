using Microsoft.AspNetCore.Mvc;
using ProjectOne.Application.Rentalagreement.CreateRentalagreement;
using ProjectOne.Application.Rentalagreement.DeleteRentalagreement;
using ProjectOne.Application.Rentalagreement.GetRentalagreement;
using ProjectOne.Application.Rentalagreement.UpdateRentalagreement;
using ProjectOne.Extensions;

namespace ProjectOne.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RentalagreementController : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> AddRentalAgreementAsync(
        [FromServices] CreateRentalagreementHandler createRentalagreementHandler,
        [FromBody] CreateRentalagreementRequest createRentalagreementRequest,
        Guid currentUserId,
        CancellationToken cancellationToken = default)
    {
        var rentalagreement = await createRentalagreementHandler.CreateAsync(
            createRentalagreementRequest, currentUserId, cancellationToken);

        return rentalagreement.ToResponse();
    }
    
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetRentalagreementResponse>> GetRentalagreementAsync(
        [FromServices] GetRentalagreementHandler handler,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.GetAsync(id, cancellationToken);
        return result.ToResponse();
    }

    [HttpGet]
    public async Task<ActionResult<List<GetRentalagreementResponse>>> GetAllRentalagreementAsync(
        [FromServices] GetAllRentalagreementHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.GetAllAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Guid>> UpdateRentalagreementAsync(
        [FromServices] UpdateRentalagreementHandler handler,
        Guid id,
        [FromBody] UpdateRentalagreementRequest request,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.UpdateAsync(id, request, cancellationToken);
        return result.ToResponse();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteRentalagreementAsync(
        [FromServices] DeleteRentalagreementHandler handler,
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.DeleteAsync(id, cancellationToken);
        return result.IsSuccess ? NoContent() : result.Error.ToResponse();
    }
}