using System;
using System.Security.Claims;
using FitnessApp.Application.DTOs.Subscriptions.Purchase;
using FitnessApp.Application.Facades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers.Subscriptions;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Client,Admin")]
public class PurchaseSubscriptionController : ControllerBase
{
     private readonly CheckoutFacade _checkout;

    public PurchaseSubscriptionController(CheckoutFacade checkout)
    {
        _checkout = checkout;
    }

    [HttpPost]
    [ProducesResponseType(typeof(PurchaseSubscriptionResultDto), 200)]
    public async Task<ActionResult<PurchaseSubscriptionResultDto>> Purchase(
        [FromBody] PurchaseSubscriptionRequestDto dto,
        CancellationToken cancellationToken)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdStr, out var userId) || userId <= 0)
            return Unauthorized(new { message = "Invalid user id in token." });

         var result = await _checkout.PurchaseSubscriptionAsync(userId, dto, cancellationToken);
        return Ok(result);
    }
}