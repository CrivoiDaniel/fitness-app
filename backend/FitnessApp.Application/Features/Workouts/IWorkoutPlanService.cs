using System;
using FitnessApp.Application.DTOs.Workouts;

namespace FitnessApp.Application.Features.Workouts;

/// <summary>
/// Service interface for WorkoutPlan using Builder Pattern
/// </summary>
public interface IWorkoutPlanService
{
    /// <summary>
    /// Creates workout plan using Builder Pattern
    /// </summary>
    Task<WorkoutPlanResponse> CreateWorkoutPlanAsync(
        CreateWorkoutPlanRequest request, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets workout plan by ID
    /// </summary>
    Task<WorkoutPlanResponse?> GetByIdAsync(
        int id, 
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all workout plans
    /// </summary>
    Task<List<WorkoutPlanResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets workout plans by client ID
    /// </summary>
    Task<List<WorkoutPlanResponse>> GetByClientIdAsync(
        int clientId, 
        CancellationToken cancellationToken = default);
}