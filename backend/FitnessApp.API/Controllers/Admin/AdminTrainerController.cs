using Microsoft.AspNetCore.Mvc;
using FitnessApp.Application.DTOs.Admin;
using FitnessApp.Application.Interfaces.Admin;

namespace FitnessApp.API.Controllers.Admin;

/// <summary>
/// Controller for Admin operations on Trainers
/// RESTful API design - Single Resource (Trainer)
/// SRP - Responsible ONLY for Trainer CRUD operations
/// </summary>
[ApiController]
[Route("api/admin/trainers")]
// TODO: Add [Authorize(Roles = "Admin")] when implementing JWT
public class AdminTrainerController : ControllerBase
{
    // ========== FIELDS ==========
    private readonly ITrainerCreationService _trainerCreationService;
    // TODO: Add ITrainerUpdateService, ITrainerDeletionService

    // ========== CONSTRUCTOR ==========
    public AdminTrainerController(ITrainerCreationService trainerCreationService)
    {
        _trainerCreationService = trainerCreationService;
    }

    // ========== PUBLIC METHODS (CRUD) ==========

    /// <summary>
    /// Admin creates a new Trainer
    /// POST /api/admin/trainers
    /// </summary>
    /// <param name="dto">Trainer creation data</param>
    /// <returns>Created trainer with temporary password</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateUserResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateUserResponseDto>> CreateTrainer(
        [FromBody] CreateTrainerDto dto)
    {
        try
        {
            var response = await _trainerCreationService.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetTrainer), 
                new { id = response.UserId }, 
                response
            );
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Admin gets Trainer by ID
    /// GET /api/admin/trainers/{id}
    /// </summary>
    /// <param name="id">Trainer's user ID</param>
    /// <returns>Trainer details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetTrainer(int id)
    {
        // TODO: Implement ITrainerQueryService.GetByIdAsync()
        return Ok(new { message = "TODO: Implement GetTrainer" });
    }

    /// <summary>
    /// Admin gets all Trainers
    /// GET /api/admin/trainers
    /// </summary>
    /// <returns>List of all trainers</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllTrainers()
    {
        // TODO: Implement ITrainerQueryService.GetAllAsync()
        return Ok(new { message = "TODO: Implement GetAllTrainers" });
    }

    /// <summary>
    /// Admin updates Trainer profile
    /// PUT /api/admin/trainers/{id}
    /// </summary>
    /// <param name="id">Trainer's user ID</param>
    /// <param name="dto">Updated trainer data</param>
    /// <returns>Updated trainer</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateTrainer(int id, [FromBody] object dto)
    {
        // TODO: Implement ITrainerUpdateService.UpdateAsync()
        return Ok(new { message = "TODO: Implement UpdateTrainer" });
    }

    /// <summary>
    /// Admin deletes Trainer (soft delete)
    /// DELETE /api/admin/trainers/{id}
    /// </summary>
    /// <param name="id">Trainer's user ID</param>
    /// <returns>Success message</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteTrainer(int id)
    {
        // TODO: Implement ITrainerDeletionService.DeleteAsync()
        return Ok(new { message = "TODO: Implement DeleteTrainer" });
    }
}