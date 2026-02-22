using FitnessApp.Application.DTOs.Subscriptions.Subscription;
using FitnessApp.Application.Interfaces.Subscriptions;
using FitnessApp.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers.Subscriptions;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;

    public SubscriptionsController(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    /// <summary>
    /// Get all subscriptions (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SubscriptionDto>>> GetAll(CancellationToken cancellationToken)
    {
        var subscriptions = await _subscriptionService.GetAllAsync(cancellationToken);
        return Ok(subscriptions);
    }

    /// <summary>
    /// Get subscriptions by client ID
    /// </summary>
    [HttpGet("client/{clientId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SubscriptionDto>>> GetByClientId(int clientId, CancellationToken cancellationToken)
    {
        var subscriptions = await _subscriptionService.GetByClientIdAsync(clientId, cancellationToken);
        return Ok(subscriptions);
    }

    /// <summary>
    /// Get active subscriptions by client ID
    /// </summary>
    [HttpGet("client/{clientId}/active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SubscriptionDto>>> GetActiveByClientId(int clientId, CancellationToken cancellationToken)
    {
        var subscriptions = await _subscriptionService.GetActiveByClientIdAsync(clientId, cancellationToken);
        return Ok(subscriptions);
    }

    /// <summary>
    /// Get subscriptions by status (Admin only)
    /// </summary>
    [HttpGet("by-status/{status}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SubscriptionDto>>> GetByStatus(SubscriptionStatus status, CancellationToken cancellationToken)
    {
        var subscriptions = await _subscriptionService.GetByStatusAsync(status, cancellationToken);
        return Ok(subscriptions);
    }

    /// <summary>
    /// Get expiring subscriptions (Admin only)
    /// </summary>
    [HttpGet("expiring")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SubscriptionDto>>> GetExpiring([FromQuery] int withinDays = 7, CancellationToken cancellationToken = default)
    {
        var subscriptions = await _subscriptionService.GetExpiringAsync(withinDays, cancellationToken);
        return Ok(subscriptions);
    }

    /// <summary>
    /// Get subscription by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SubscriptionDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionService.GetByIdAsync(id, cancellationToken);
        
        if (subscription == null)
            return NotFound(new { message = $"Subscription with ID {id} not found." });

        return Ok(subscription);
    }

    /// <summary>
    /// Get subscription with details
    /// </summary>
    [HttpGet("{id}/with-details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SubscriptionDto>> GetWithDetails(int id, CancellationToken cancellationToken)
    {
        var subscription = await _subscriptionService.GetWithDetailsAsync(id, cancellationToken);
        
        if (subscription == null)
            return NotFound(new { message = $"Subscription with ID {id} not found." });

        return Ok(subscription);
    }

    /// <summary>
    /// Create new subscription (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SubscriptionDto>> Create([FromBody] CreateSubscriptionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _subscriptionService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update subscription (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SubscriptionDto>> Update(int id, [FromBody] UpdateSubscriptionDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _subscriptionService.UpdateAsync(id, dto, cancellationToken);
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Cancel subscription
    /// </summary>
    [HttpPost("{id}/cancel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SubscriptionDto>> Cancel(int id, CancellationToken cancellationToken)
    {
        try
        {
            var cancelled = await _subscriptionService.CancelAsync(id, cancellationToken);
            return Ok(cancelled);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Renew subscription
    /// </summary>
    [HttpPost("{id}/renew")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SubscriptionDto>> Renew(int id, CancellationToken cancellationToken)
    {
        try
        {
            var renewed = await _subscriptionService.RenewAsync(id, cancellationToken);
            return Ok(renewed);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete subscription (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _subscriptionService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}