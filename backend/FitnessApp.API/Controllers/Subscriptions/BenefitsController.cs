using FitnessApp.Application.DTOs.Subscriptions.Benefit;
using FitnessApp.Application.Interfaces.Subscriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers.Subscriptions;
[ApiController]
[Route("api/[controller]")]
//[Authorize] // Doar utilizatori autentificați
public class BenefitsController : ControllerBase
{
    private readonly IBenefitService _benefitService;

    public BenefitsController(IBenefitService benefitService)
    {
        _benefitService = benefitService;
    }

    /// <summary>
    /// Get all benefits
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BenefitDto>>> GetAll(CancellationToken cancellationToken)
    {
        var benefits = await _benefitService.GetAllAsync(cancellationToken);
        return Ok(benefits);
    }

    /// <summary>
    /// Get active benefits only
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BenefitDto>>> GetActive(CancellationToken cancellationToken)
    {
        var benefits = await _benefitService.GetActiveAsync(cancellationToken);
        return Ok(benefits);
    }

    /// <summary>
    /// Get benefit by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BenefitDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var benefit = await _benefitService.GetByIdAsync(id, cancellationToken);
        
        if (benefit == null)
            return NotFound(new { message = $"Benefit with ID {id} not found." });

        return Ok(benefit);
    }

    /// <summary>
    /// Get benefit by name
    /// </summary>
    [HttpGet("by-name/{name}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BenefitDto>> GetByName(string name, CancellationToken cancellationToken)
    {
        var benefit = await _benefitService.GetByNameAsync(name, cancellationToken);
        
        if (benefit == null)
            return NotFound(new { message = $"Benefit with name '{name}' not found." });

        return Ok(benefit);
    }

    /// <summary>
    /// Create new benefit (Admin only)
    /// </summary>
    [HttpPost]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BenefitDto>> Create([FromBody] CreateBenefitDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _benefitService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update benefit (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BenefitDto>> Update(int id, [FromBody] UpdateBenefitDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _benefitService.UpdateAsync(id, dto, cancellationToken);
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete benefit (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    //[Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _benefitService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}