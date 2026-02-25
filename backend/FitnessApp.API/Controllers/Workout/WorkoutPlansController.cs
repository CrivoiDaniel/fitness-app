using FitnessApp.Application.DTOs.Common;
using FitnessApp.Application.DTOs.Workouts;
using FitnessApp.Application.Features.Workouts;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Workout Plans API Controller
/// Demonstrates Builder Pattern for complex object construction
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class WorkoutPlansController : ControllerBase
{
    private readonly IWorkoutPlanService _workoutPlanService;

    public WorkoutPlansController(IWorkoutPlanService workoutPlanService)
    {
        _workoutPlanService = workoutPlanService;
    }

    /// <summary>
    /// Creates a new workout plan using Builder Pattern
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(WorkoutPlanResponse), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<ActionResult<WorkoutPlanResponse>> CreateWorkoutPlan(
        [FromBody] CreateWorkoutPlanRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _workoutPlanService.CreateWorkoutPlanAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetWorkoutPlan),
                new { id = response.Id },
                response
            );
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ErrorResponse
            {
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponse
            {
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse
            {
                Message = "An error occurred while creating workout plan",
                Details = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    /// Gets a specific workout plan by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(WorkoutPlanResponse), 200)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<ActionResult<WorkoutPlanResponse>> GetWorkoutPlan(
        int id,
        CancellationToken cancellationToken)
    {
        var workoutPlan = await _workoutPlanService.GetByIdAsync(id, cancellationToken);

        if (workoutPlan == null)
            return NotFound(new ErrorResponse
            {
                Message = $"Workout plan with ID {id} not found",
                Timestamp = DateTime.UtcNow
            });

        return Ok(workoutPlan);
    }

    /// <summary>
    /// Gets all workout plans in the system
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<WorkoutPlanResponse>), 200)]
    public async Task<ActionResult<List<WorkoutPlanResponse>>> GetAllWorkoutPlans(
        CancellationToken cancellationToken)
    {
        var workoutPlans = await _workoutPlanService.GetAllAsync(cancellationToken);
        return Ok(workoutPlans);
    }

    /// <summary>
    /// Gets all workout plans for a specific client
    /// </summary>
    [HttpGet("client/{clientId}")]
    [ProducesResponseType(typeof(List<WorkoutPlanResponse>), 200)]
    public async Task<ActionResult<List<WorkoutPlanResponse>>> GetWorkoutPlansByClient(
        int clientId,
        CancellationToken cancellationToken)
    {
        var workoutPlans = await _workoutPlanService.GetByClientIdAsync(clientId, cancellationToken);
        return Ok(workoutPlans);
    }

    /// <summary>
    /// Clones an existing workout plan for a different client
    /// Demonstrates Prototype Pattern
    /// </summary>
    [HttpPost("clone")]
    [ProducesResponseType(typeof(WorkoutPlanResponse), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 400)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<ActionResult<WorkoutPlanResponse>> CloneWorkoutPlan(
        [FromBody] CloneWorkoutPlanRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _workoutPlanService.CloneWorkoutPlanAsync(request, cancellationToken);

            return CreatedAtAction(
                nameof(GetWorkoutPlan),
                new { id = response.Id },
                response
            );
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ErrorResponse
            {
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ErrorResponse
            {
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse
            {
                Message = "An error occurred while cloning workout plan",
                Details = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    /// Clones a workout plan as a new template
    /// </summary>
    [HttpPost("{sourceId}/clone-as-template")]
    [ProducesResponseType(typeof(WorkoutPlanResponse), 201)]
    [ProducesResponseType(typeof(ErrorResponse), 404)]
    public async Task<ActionResult<WorkoutPlanResponse>> CloneAsTemplate(
        int sourceId,
        [FromQuery] string newName,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(newName))
                return BadRequest(new ErrorResponse
                {
                    Message = "New name is required",
                    Timestamp = DateTime.UtcNow
                });

            var response = await _workoutPlanService.CloneAsTemplateAsync(
                sourceId,
                newName,
                cancellationToken
            );

            return CreatedAtAction(
                nameof(GetWorkoutPlan),
                new { id = response.Id },
                response
            );
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ErrorResponse
            {
                Message = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ErrorResponse
            {
                Message = "An error occurred while cloning template",
                Details = ex.Message,
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
