using FitnessApp.Application.DTOs.Subscriptions.SubscriptionPlan;
using FitnessApp.Application.Interfaces.Subscriptions;
using FitnessApp.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers.Subscriptions;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionPlansController : ControllerBase
{
    private readonly ISubscriptionPlanService _planService;

    public SubscriptionPlansController(ISubscriptionPlanService planService)
    {
        _planService = planService;
    }

    /// <summary>
    /// Get all subscription plans
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SubscriptionPlanDto>>> GetAll(CancellationToken cancellationToken)
    {
        var plans = await _planService.GetAllAsync(cancellationToken);
        return Ok(plans);
    }

    /// <summary>
    /// Get active subscription plans only
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SubscriptionPlanDto>>> GetActive(CancellationToken cancellationToken)
    {
        var plans = await _planService.GetActiveAsync(cancellationToken);
        return Ok(plans);
    }

    /// <summary>
    /// Get subscription plans by type
    /// </summary>
    [HttpGet("by-type/{type}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<SubscriptionPlanDto>>> GetByType(SubscriptionType type, CancellationToken cancellationToken)
    {
        var plans = await _planService.GetByTypeAsync(type, cancellationToken);
        return Ok(plans);
    }

    /// <summary>
    /// Get subscription plan by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SubscriptionPlanDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var plan = await _planService.GetByIdAsync(id, cancellationToken);
        
        if (plan == null)
            return NotFound(new { message = $"Subscription plan with ID {id} not found." });

        return Ok(plan);
    }

    /// <summary>
    /// Get subscription plan with details
    /// </summary>
    [HttpGet("{id}/with-details")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<SubscriptionPlanDto>> GetWithDetails(int id, CancellationToken cancellationToken)
    {
        var plan = await _planService.GetWithDetailsAsync(id, cancellationToken);
        
        if (plan == null)
            return NotFound(new { message = $"Subscription plan with ID {id} not found." });

        return Ok(plan);
    }

    /// <summary>
    /// Create new subscription plan (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SubscriptionPlanDto>> Create([FromBody] CreateSubscriptionPlanDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _planService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update subscription plan (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<SubscriptionPlanDto>> Update(int id, [FromBody] UpdateSubscriptionPlanDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _planService.UpdateAsync(id, dto, cancellationToken);
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
    /// Delete subscription plan (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _planService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}