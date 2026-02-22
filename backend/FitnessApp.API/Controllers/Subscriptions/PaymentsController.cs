using FitnessApp.Application.DTOs.Subscriptions.Payment;
using FitnessApp.Application.Interfaces.Subscriptions;
using FitnessApp.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers.Subscriptions;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    /// Get all payments (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> GetAll(CancellationToken cancellationToken)
    {
        var payments = await _paymentService.GetAllAsync(cancellationToken);
        return Ok(payments);
    }

    /// <summary>
    /// Get payments by subscription ID
    /// </summary>
    [HttpGet("subscription/{subscriptionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> GetBySubscriptionId(int subscriptionId, CancellationToken cancellationToken)
    {
        var payments = await _paymentService.GetBySubscriptionIdAsync(subscriptionId, cancellationToken);
        return Ok(payments);
    }

    /// <summary>
    /// Get payments by client ID
    /// </summary>
    [HttpGet("client/{clientId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> GetByClientId(int clientId, CancellationToken cancellationToken)
    {
        var payments = await _paymentService.GetByClientIdAsync(clientId, cancellationToken);
        return Ok(payments);
    }

    /// <summary>
    /// Get payments by status (Admin only)
    /// </summary>
    [HttpGet("by-status/{status}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> GetByStatus(PaymentStatus status, CancellationToken cancellationToken)
    {
        var payments = await _paymentService.GetByStatusAsync(status, cancellationToken);
        return Ok(payments);
    }

    /// <summary>
    /// Get pending payments (Admin only)
    /// </summary>
    [HttpGet("pending")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PaymentDto>>> GetPending(CancellationToken cancellationToken)
    {
        var payments = await _paymentService.GetPendingPaymentsAsync(cancellationToken);
        return Ok(payments);
    }

    /// <summary>
    /// Get payment by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaymentDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var payment = await _paymentService.GetByIdAsync(id, cancellationToken);
        
        if (payment == null)
            return NotFound(new { message = $"Payment with ID {id} not found." });

        return Ok(payment);
    }

    /// <summary>
    /// Get payment by transaction ID
    /// </summary>
    [HttpGet("transaction/{transactionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaymentDto>> GetByTransactionId(string transactionId, CancellationToken cancellationToken)
    {
        var payment = await _paymentService.GetByTransactionIdAsync(transactionId, cancellationToken);
        
        if (payment == null)
            return NotFound(new { message = $"Payment with transaction ID '{transactionId}' not found." });

        return Ok(payment);
    }

    /// <summary>
    /// Get total revenue (Admin only)
    /// </summary>
    [HttpGet("revenue")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<decimal>> GetTotalRevenue(
        [FromQuery] DateTime? startDate = null, 
        [FromQuery] DateTime? endDate = null, 
        CancellationToken cancellationToken = default)
    {
        var revenue = await _paymentService.GetTotalRevenueAsync(startDate, endDate, cancellationToken);
        return Ok(new { totalRevenue = revenue, startDate, endDate });
    }

    /// <summary>
    /// Create new payment (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaymentDto>> Create([FromBody] CreatePaymentDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _paymentService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update payment (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaymentDto>> Update(int id, [FromBody] UpdatePaymentDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _paymentService.UpdateAsync(id, dto, cancellationToken);
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
    /// Mark payment as success (Admin only)
    /// </summary>
    [HttpPost("{id}/mark-success")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaymentDto>> MarkAsSuccess(int id, [FromQuery] string? transactionId = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var payment = await _paymentService.MarkAsSuccessAsync(id, transactionId, cancellationToken);
            return Ok(payment);
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
    /// Mark payment as failed (Admin only)
    /// </summary>
    [HttpPost("{id}/mark-failed")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PaymentDto>> MarkAsFailed(int id, CancellationToken cancellationToken)
    {
        try
        {
            var payment = await _paymentService.MarkAsFailedAsync(id, cancellationToken);
            return Ok(payment);
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
    /// Delete payment (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _paymentService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}