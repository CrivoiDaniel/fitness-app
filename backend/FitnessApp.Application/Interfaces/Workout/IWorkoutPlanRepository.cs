using System;
using FitnessApp.Domain.Entities.Workouts;

namespace FitnessApp.Application.Interfaces.Workout;

/// <summary>
/// Repository interface for WorkoutPlan
/// </summary>
public interface IWorkoutPlanRepository
{
    Task<WorkoutPlan?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<WorkoutPlan?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task<List<WorkoutPlan>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<WorkoutPlan>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default);
    Task<List<WorkoutPlan>> GetByTrainerIdAsync(int trainerId, CancellationToken cancellationToken = default);
    Task<WorkoutPlan> AddAsync(WorkoutPlan workoutPlan, CancellationToken cancellationToken = default);
    Task UpdateAsync(WorkoutPlan workoutPlan, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}