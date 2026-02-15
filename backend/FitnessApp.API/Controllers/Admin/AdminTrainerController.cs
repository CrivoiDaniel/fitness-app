using Microsoft.AspNetCore.Mvc;
using FitnessApp.Application.DTOs.Admin.Trainers;
using FitnessApp.Application.DTOs.Admin;
using FitnessApp.Application.Interfaces.Admin.Trainers;

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
    private readonly ITrainerQueryService _trainerQueryService;
    private readonly ITrainerUpdateService _trainerUpdateService;
    private readonly ITrainerDeleteService _trainerDeletionService;

    // TODO: Add ITrainerUpdateService, ITrainerDeletionService

    // ========== CONSTRUCTOR ==========
    public AdminTrainerController(ITrainerCreationService trainerCreationService, ITrainerQueryService trainerQueryService,  ITrainerUpdateService trainerUpdateService,
        ITrainerDeleteService trainerDeletionService)
    {
        _trainerCreationService = trainerCreationService;
        _trainerQueryService = trainerQueryService;
        _trainerUpdateService = trainerUpdateService;
        _trainerDeletionService = trainerDeletionService;
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
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TrainerDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TrainerDetailsDto>> GetTrainer([FromRoute] int id)
    {
        var trainer = await _trainerQueryService.GetByIdAsync(id);
        
        if (trainer == null)
            return NotFound(new { message = $"Trainer with user ID {id} not found" });
        
        return Ok(trainer);
    }

    /// <summary>
    /// Admin gets all Trainers
    /// GET /api/admin/trainers
    /// </summary>
    /// <returns>List of all trainers</returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<TrainerDetailsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TrainerDetailsDto>>> GetAllTrainers()
    {
        var Trainers = await _trainerQueryService.GetAllAsync();
        return Ok(Trainers);
    }

    /// <summary>
    /// Admin updates Trainer profile
    /// PUT /api/admin/trainers/{id}
    /// </summary>
    /// <param name="id">Trainer's user ID</param>
    /// <param name="dto">Updated trainer data</param>
    /// <returns>Updated trainer</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TrainerDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TrainerDetailsDto>> UpdateTrainer(
        [FromRoute] int id,
        [FromBody] UpdateTrainerDto dto)
    {
        try
        {
            var updatedTrainer = await _trainerUpdateService.UpdateAsync(id, dto);
            return Ok(updatedTrainer);
        }
        catch (InvalidOperationException ex)
        {
            // Trainer not found OR Email already in use
            if (ex.Message.Contains("not found"))
                return NotFound(new { message = ex.Message });
            else
                return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    /// <summary>
    /// Admin deletes Trainer (soft delete)
    /// DELETE /api/admin/trainers/{id}
    /// </summary>
    /// <param name="id">Trainer's user ID</param>
    /// <returns>Success message</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteTrainer([FromRoute] int id)
    {
        try
        {
            await _trainerDeletionService.DeleteAsync(id);
            return Ok(new 
            { 
                message = $"Trainer with user ID {id} has been permanently deleted",
                warning = "This action cannot be undone!"
            });
        }
        catch (InvalidOperationException e)
        {
            if (e.Message.Contains("not found"))
                return NotFound(new { message = e.Message });
            else
                return BadRequest(new { message = e.Message });
        }
    }
}