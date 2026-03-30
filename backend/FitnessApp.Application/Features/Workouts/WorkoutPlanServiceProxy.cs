using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FitnessApp.Application.DTOs.Workouts;
using Microsoft.AspNetCore.Http;

namespace FitnessApp.Application.Features.Workouts;

/// <summary>
/// Protection Proxy for IWorkoutPlanService.
/// Intercepts requests and enforces Role-Based Access Control before delegating to the real service.
/// </summary>
public class WorkoutPlanServiceProxy : IWorkoutPlanService
{
    private readonly IWorkoutPlanService _innerService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkoutPlanServiceProxy(IWorkoutPlanService innerService, IHttpContextAccessor httpContextAccessor)
    {
        _innerService = innerService;
        _httpContextAccessor = httpContextAccessor;
    }

    private void EnsureIsTrainerOrAdmin()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user == null || !user.Identity!.IsAuthenticated)
            throw new UnauthorizedAccessException("Nu este niciun utilizator autentificat in sesiune.");

        var role = user.FindFirst(ClaimTypes.Role)?.Value;

        // Protection check: Only Trainers and Admins can mutate Workout Plans
        if (role != "Trainer" && role != "Admin")
        {
            throw new UnauthorizedAccessException("Acces respins: Doar un Trainer sau un Admin are permisiunea de a crea, clona sau oferi șabloane de antrenament.");
        }
    }

    // --- PROTECTED METHODS (Only Trainers and Admins) ---

    public Task<WorkoutPlanResponse> CreateWorkoutPlanAsync(CreateWorkoutPlanRequest request, CancellationToken cancellationToken = default)
    {
        EnsureIsTrainerOrAdmin(); // Proxy intercepts here
        return _innerService.CreateWorkoutPlanAsync(request, cancellationToken);
    }

    public Task<WorkoutPlanResponse> CloneWorkoutPlanAsync(CloneWorkoutPlanRequest request, CancellationToken cancellationToken = default)
    {
        EnsureIsTrainerOrAdmin();
        return _innerService.CloneWorkoutPlanAsync(request, cancellationToken);
    }

    public Task<WorkoutPlanResponse> CloneAsTemplateAsync(int sourceId, string newName, CancellationToken cancellationToken = default)
    {
        EnsureIsTrainerOrAdmin();
        return _innerService.CloneAsTemplateAsync(sourceId, newName, cancellationToken);
    }

    public Task<WorkoutPlanResponse> CreateBeginnerFullBodyAsync(int clientId, CancellationToken cancellationToken = default)
    {
        EnsureIsTrainerOrAdmin();
        return _innerService.CreateBeginnerFullBodyAsync(clientId, cancellationToken);
    }

    public Task<WorkoutPlanResponse> CreateIntermediateStrengthAsync(int clientId, CancellationToken cancellationToken = default)
    {
        EnsureIsTrainerOrAdmin();
        return _innerService.CreateIntermediateStrengthAsync(clientId, cancellationToken);
    }

    public Task<WorkoutPlanResponse> CreateAdvancedMuscleGainAsync(int clientId, CancellationToken cancellationToken = default)
    {
        EnsureIsTrainerOrAdmin();
        return _innerService.CreateAdvancedMuscleGainAsync(clientId, cancellationToken);
    }

    public Task<WorkoutPlanResponse> CreateWeightLossProgramAsync(int clientId, CancellationToken cancellationToken = default)
    {
        EnsureIsTrainerOrAdmin();
        return _innerService.CreateWeightLossProgramAsync(clientId, cancellationToken);
    }

    public Task<WorkoutPlanResponse> CreateEnduranceProgramAsync(int clientId, CancellationToken cancellationToken = default)
    {
        EnsureIsTrainerOrAdmin();
        return _innerService.CreateEnduranceProgramAsync(clientId, cancellationToken);
    }

    // --- FREELY ACCESSIBLE METHODS (Pass-through to Real Service) ---

    public Task<WorkoutPlanResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return _innerService.GetByIdAsync(id, cancellationToken);
    }

    public Task<List<WorkoutPlanResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _innerService.GetAllAsync(cancellationToken);
    }

    public Task<List<WorkoutPlanResponse>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default)
    {
        return _innerService.GetByClientIdAsync(clientId, cancellationToken);
    }

    public Task<List<WorkoutPlanResponse>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return _innerService.GetByUserIdAsync(userId, cancellationToken);
    }

    public Task<(byte[] FileBytes, string ContentType, string FileName)> ExportWorkoutPlanAsync(int id, string format, string detailLevel, CancellationToken cancellationToken = default)
    {
        // Clients can export their plans freely
        return _innerService.ExportWorkoutPlanAsync(id, format, detailLevel, cancellationToken);
    }
}
