using System;
using FitnessApp.Application.DTOs.Workouts;

namespace FitnessApp.Application.Features.Workouts;

public interface IWorkoutPlanService
{
    Task<WorkoutPlanResponse> CreateWorkoutPlanAsync(CreateWorkoutPlanRequest request, CancellationToken cancellationToken = default);
    Task<WorkoutPlanResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<WorkoutPlanResponse>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<List<WorkoutPlanResponse>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default);
    
    // PROTOTYPE PATTERN
    Task<WorkoutPlanResponse> CloneWorkoutPlanAsync(CloneWorkoutPlanRequest request, CancellationToken cancellationToken = default);
    Task<WorkoutPlanResponse> CloneAsTemplateAsync(int sourceId, string newName, CancellationToken cancellationToken = default);

     //DIRECTOR METHODS
    Task<WorkoutPlanResponse> CreateBeginnerFullBodyAsync(int clientId, CancellationToken cancellationToken = default);
    Task<WorkoutPlanResponse> CreateIntermediateStrengthAsync(int clientId, CancellationToken cancellationToken = default);
    Task<WorkoutPlanResponse> CreateAdvancedMuscleGainAsync(int clientId, CancellationToken cancellationToken = default);
    Task<WorkoutPlanResponse> CreateWeightLossProgramAsync(int clientId, CancellationToken cancellationToken = default);
    Task<WorkoutPlanResponse> CreateEnduranceProgramAsync(int clientId, CancellationToken cancellationToken = default);
}