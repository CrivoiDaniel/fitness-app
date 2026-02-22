using FitnessApp.Application.DTOs.Subscriptions.BenefitPackage;
using FitnessApp.Application.Interfaces.Subscriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers.Subscriptions
{
    
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BenefitPackagesController : ControllerBase
{
    private readonly IBenefitPackageService _packageService;

    public BenefitPackagesController(IBenefitPackageService packageService)
    {
        _packageService = packageService;
    }

    /// <summary>
    /// Get all benefit packages
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BenefitPackageDto>>> GetAll(CancellationToken cancellationToken)
    {
        var packages = await _packageService.GetAllAsync(cancellationToken);
        return Ok(packages);
    }

    /// <summary>
    /// Get active benefit packages only
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<BenefitPackageDto>>> GetActive(CancellationToken cancellationToken)
    {
        var packages = await _packageService.GetActiveAsync(cancellationToken);
        return Ok(packages);
    }

    /// <summary>
    /// Get benefit package by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BenefitPackageDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var package = await _packageService.GetByIdAsync(id, cancellationToken);
        
        if (package == null)
            return NotFound(new { message = $"Benefit package with ID {id} not found." });

        return Ok(package);
    }

    /// <summary>
    /// Get benefit package with items
    /// </summary>
    [HttpGet("{id}/with-items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BenefitPackageDto>> GetWithItems(int id, CancellationToken cancellationToken)
    {
        var package = await _packageService.GetWithItemsAsync(id, cancellationToken);
        
        if (package == null)
            return NotFound(new { message = $"Benefit package with ID {id} not found." });

        return Ok(package);
    }

    /// <summary>
    /// Create new benefit package (Admin only)
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BenefitPackageDto>> Create([FromBody] CreateBenefitPackageDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var created = await _packageService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update benefit package (Admin only)
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BenefitPackageDto>> Update(int id, [FromBody] UpdateBenefitPackageDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var updated = await _packageService.UpdateAsync(id, dto, cancellationToken);
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
    /// Delete benefit package (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _packageService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
}
