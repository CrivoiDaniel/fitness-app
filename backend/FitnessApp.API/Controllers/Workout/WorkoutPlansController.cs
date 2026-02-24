using System;
using FitnessApp.Application.DTOs.Workouts;
using FitnessApp.Application.Features.Workouts;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers.Workout;

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
    /// <param name="id">Workout plan ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="200">Workout plan found and returned</response>
    /// <response code="404">Workout plan not found</response>
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
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="200">List of all workout plans (can be empty)</response>
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
    /// <param name="clientId">Client ID to filter by</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <response code="200">List of workout plans for the client (can be empty)</response>
    [HttpGet("client/{clientId}")]
    [ProducesResponseType(typeof(List<WorkoutPlanResponse>), 200)]
    public async Task<ActionResult<List<WorkoutPlanResponse>>> GetWorkoutPlansByClient(
        int clientId,
        CancellationToken cancellationToken)
    {
        var workoutPlans = await _workoutPlanService.GetByClientIdAsync(clientId, cancellationToken);
        return Ok(workoutPlans);
    }
}

/// <summary>
/// Standard error response model
/// </summary>
public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; }
}