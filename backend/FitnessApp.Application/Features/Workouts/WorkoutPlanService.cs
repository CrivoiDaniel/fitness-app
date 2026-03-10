using System;
using FitnessApp.Application.DTOs.Workouts;
using FitnessApp.Application.Interfaces.Repositories.Users;
using FitnessApp.Application.Interfaces.Workout;
using FitnessApp.Domain.Builders;
using FitnessApp.Domain.Directors;
using FitnessApp.Domain.Entities.Workouts;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Features.Workouts;


/// <summary>
/// Service for creating and managing WorkoutPlans using Builder Pattern
/// Flow: Request DTO → Builder → Entity → Repository → Response DTO
/// </summary>
public class WorkoutPlanService : IWorkoutPlanService
{
    private readonly IWorkoutPlanRepository _workoutPlanRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ITrainerRepository _trainerRepository;
    private readonly WorkoutPlanBuilder _builder;
    private readonly WorkoutPlanDirector _director;

    public WorkoutPlanService(
    IWorkoutPlanRepository workoutPlanRepository,
    IClientRepository clientRepository,
    ITrainerRepository trainerRepository)
    {
        _workoutPlanRepository = workoutPlanRepository;
        _clientRepository = clientRepository;
        _trainerRepository = trainerRepository;

        // Initialize Builder and Director
        _builder = new WorkoutPlanBuilder();
        _director = new WorkoutPlanDirector(_builder);
    }

    /// <summary>
    /// Creates workout plan using Builder Pattern
    /// </summary>
    public async Task<WorkoutPlanResponse> CreateWorkoutPlanAsync(
        CreateWorkoutPlanRequest request,
        CancellationToken cancellationToken = default)
    {
        // STEP 1: Validate client exists
        var client = await _clientRepository.GetByIdWithDetailsAsync(request.ClientId, cancellationToken);
        if (client == null)
            throw new InvalidOperationException($"Client with ID {request.ClientId} not found");

        // STEP 2: Validate trainer exists (if provided)
        if (request.TrainerId.HasValue)
        {
            var trainer = await _trainerRepository.GetByIdAsync(request.TrainerId.Value, cancellationToken);
            if (trainer == null)
                throw new InvalidOperationException($"Trainer with ID {request.TrainerId.Value} not found");
        }

        // STEP 3: Use Builder to construct WorkoutPlan
        _builder.Reset();

        _builder
            .WithName(request.Name)
            .ForClient(request.ClientId)
            .WithGoal(request.Goal)
            .WithDifficulty(request.Difficulty)
            .WithDuration(request.DurationWeeks)
            .OnDays(request.WorkoutDays.ToArray())
            .WithSessionDuration(request.SessionDurationMinutes);

        // Optional configurations
        if (request.TrainerId.HasValue)
            _builder.WithTrainer(request.TrainerId.Value);

        if (!string.IsNullOrWhiteSpace(request.Description))
            _builder.WithDescription(request.Description);

        if (request.RestDays.HasValue)
            _builder.WithRestDays(request.RestDays.Value);

        if (!string.IsNullOrWhiteSpace(request.SpecialNotes))
            _builder.WithNotes(request.SpecialNotes);

        // Add exercises
        foreach (var exercise in request.Exercises)
        {
            _builder.AddExercise(
                name: exercise.Name,
                sets: exercise.Sets,
                reps: exercise.Reps,
                durationSeconds: exercise.DurationSeconds,
                notes: exercise.Notes
            );
        }

        // STEP 4: Build the WorkoutPlan entity
        var workoutPlan = _builder.Build();

        // STEP 5: Save to database
        var savedWorkoutPlan = await _workoutPlanRepository.AddAsync(workoutPlan, cancellationToken);

        // STEP 6: Reload with all relationships
        var fullWorkoutPlan = await _workoutPlanRepository.GetByIdWithDetailsAsync(
            savedWorkoutPlan.Id,
            cancellationToken
        );

        // STEP 7: Map to response DTO
        return MapToResponse(fullWorkoutPlan!);
    }

    /// <summary>
    /// Gets workout plan by ID
    /// </summary>
    public async Task<WorkoutPlanResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var workoutPlan = await _workoutPlanRepository.GetByIdWithDetailsAsync(id, cancellationToken);
        return workoutPlan == null ? null : MapToResponse(workoutPlan);
    }

    /// <summary>
    /// Gets all workout plans
    /// </summary>
    public async Task<List<WorkoutPlanResponse>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var workoutPlans = await _workoutPlanRepository.GetAllAsync(cancellationToken);
        return workoutPlans.Select(MapToResponse).ToList();
    }

    /// <summary>
    /// Gets all workout plans for a specific client
    /// </summary>
    public async Task<List<WorkoutPlanResponse>> GetByClientIdAsync(int clientId, CancellationToken cancellationToken = default)
    {
        var workoutPlans = await _workoutPlanRepository.GetByClientIdAsync(clientId, cancellationToken);
        return workoutPlans.Select(MapToResponse).ToList();
    }

    /// <summary>
    /// Maps WorkoutPlan entity to Response DTO
    /// </summary>
    private WorkoutPlanResponse MapToResponse(WorkoutPlan workoutPlan)
    {
        var clientName = workoutPlan.Client?.User?.GetFullName() ?? "Unknown";
        var trainerName = workoutPlan.Trainer?.User?.GetFullName();

        // Parse workout days from flags
        var workoutDays = new List<string>();
        if ((workoutPlan.WorkoutDays & DayOfWeekFlag.Monday) != 0) workoutDays.Add("Monday");
        if ((workoutPlan.WorkoutDays & DayOfWeekFlag.Tuesday) != 0) workoutDays.Add("Tuesday");
        if ((workoutPlan.WorkoutDays & DayOfWeekFlag.Wednesday) != 0) workoutDays.Add("Wednesday");
        if ((workoutPlan.WorkoutDays & DayOfWeekFlag.Thursday) != 0) workoutDays.Add("Thursday");
        if ((workoutPlan.WorkoutDays & DayOfWeekFlag.Friday) != 0) workoutDays.Add("Friday");
        if ((workoutPlan.WorkoutDays & DayOfWeekFlag.Saturday) != 0) workoutDays.Add("Saturday");
        if ((workoutPlan.WorkoutDays & DayOfWeekFlag.Sunday) != 0) workoutDays.Add("Sunday");

        var response = new WorkoutPlanResponse
        {
            Id = workoutPlan.Id,
            Name = workoutPlan.Name,
            Description = workoutPlan.Description,

            ClientId = workoutPlan.ClientId,
            ClientName = clientName,

            TrainerId = workoutPlan.TrainerId,
            TrainerName = trainerName,

            Goal = workoutPlan.Goal.ToString(),
            Difficulty = workoutPlan.Difficulty.ToString(),
            DurationWeeks = workoutPlan.DurationWeeks,

            WorkoutDays = workoutDays,
            SessionsPerWeek = workoutPlan.SessionsPerWeek,
            SessionDurationMinutes = workoutPlan.SessionDurationMinutes,
            RestDaysBetweenSessions = workoutPlan.RestDaysBetweenSessions,

            TotalSessions = workoutPlan.TotalSessions(),
            TotalMinutes = workoutPlan.TotalMinutes(),

            Exercises = workoutPlan.Exercises?
            .OrderBy(e => e.OrderInWorkout)
            .Select(e => new ExerciseResponse
            {
                Id = e.Id,
                Name = e.ExerciseName,
                Sets = e.Sets,
                Reps = e.Reps,
                DurationSeconds = e.DurationSeconds,
                OrderInWorkout = e.OrderInWorkout,
                Notes = e.Notes
            })
            .ToList() ?? new List<ExerciseResponse>(),

            IsActive = workoutPlan.IsActive,
            SpecialNotes = workoutPlan.SpecialNotes,
            CreatedAt = workoutPlan.CreatedAt
        };

        // Composite computed (după ce ai implementat GetTotalSets/GetTotalReps/GetTotalDurationSeconds pe entity)
        response.TotalSets = workoutPlan.GetTotalSets();
        response.TotalReps = workoutPlan.GetTotalReps();
        response.TotalExerciseDurationSeconds = workoutPlan.GetTotalDurationSeconds();

        return response;
    }


    // ========== PROTOTYPE PATTERN METHODS ==========

    /// <summary>
    /// Clones an existing workout plan for a different client
    /// Demonstrates Prototype Pattern
    /// </summary>
    public async Task<WorkoutPlanResponse> CloneWorkoutPlanAsync(
        CloneWorkoutPlanRequest request,
        CancellationToken cancellationToken = default)
    {
        // STEP 1: Get source workout plan
        var sourceWorkoutPlan = await _workoutPlanRepository.GetByIdWithDetailsAsync(
            request.SourceWorkoutPlanId,
            cancellationToken
        );

        if (sourceWorkoutPlan == null)
            throw new InvalidOperationException($"Source workout plan with ID {request.SourceWorkoutPlanId} not found");

        // STEP 2: Validate target client exists
        var targetClient = await _clientRepository.GetByIdWithDetailsAsync(
            request.TargetClientId,
            cancellationToken
        );

        if (targetClient == null)
            throw new InvalidOperationException($"Target client with ID {request.TargetClientId} not found");

        // STEP 3: Validate trainer exists (if provided)
        if (request.NewTrainerId.HasValue)
        {
            var trainer = await _trainerRepository.GetByIdAsync(request.NewTrainerId.Value, cancellationToken);
            if (trainer == null)
                throw new InvalidOperationException($"Trainer with ID {request.NewTrainerId.Value} not found");
        }

        // STEP 4: CLONE using Prototype Pattern
        var clonedWorkoutPlan = sourceWorkoutPlan.CloneForClient(request.TargetClientId);

        // STEP 5: Apply modifications
        if (!string.IsNullOrWhiteSpace(request.NewName))
        {
            clonedWorkoutPlan.UpdateName(request.NewName);
        }
        else
        {
            // Default name: original name + "(Copy)"
            clonedWorkoutPlan.UpdateName($"{sourceWorkoutPlan.Name} (Copy)");
        }

        if (request.NewTrainerId.HasValue)
        {
            clonedWorkoutPlan.AssignTrainer(request.NewTrainerId.Value);
        }

        // STEP 6: Save cloned workout plan
        var savedWorkoutPlan = await _workoutPlanRepository.AddAsync(clonedWorkoutPlan, cancellationToken);

        // STEP 7: Reload with relationships
        var fullWorkoutPlan = await _workoutPlanRepository.GetByIdWithDetailsAsync(
            savedWorkoutPlan.Id,
            cancellationToken
        );

        // STEP 8: Return response
        return MapToResponse(fullWorkoutPlan!);
    }

    /// <summary>
    /// Clones a workout plan as a template (no specific client)
    /// Useful for creating seasonal programs or template libraries
    /// </summary>
    public async Task<WorkoutPlanResponse> CloneAsTemplateAsync(
        int sourceId,
        string newName,
        CancellationToken cancellationToken = default)
    {
        // STEP 1: Get source workout plan
        var sourceWorkoutPlan = await _workoutPlanRepository.GetByIdWithDetailsAsync(
            sourceId,
            cancellationToken
        );

        if (sourceWorkoutPlan == null)
            throw new InvalidOperationException($"Source workout plan with ID {sourceId} not found");

        // STEP 2: CLONE using Prototype Pattern
        var clonedWorkoutPlan = sourceWorkoutPlan.CloneWithName(newName);

        // STEP 3: Save as template
        var savedWorkoutPlan = await _workoutPlanRepository.AddAsync(clonedWorkoutPlan, cancellationToken);

        // STEP 4: Reload with relationships
        var fullWorkoutPlan = await _workoutPlanRepository.GetByIdWithDetailsAsync(
            savedWorkoutPlan.Id,
            cancellationToken
        );

        // STEP 5: Return response
        return MapToResponse(fullWorkoutPlan!);
    }

    // ========== DIRECTOR PATTERN METHODS ==========

    /// <summary>
    /// Creates a Beginner Full Body workout plan using Director
    /// Director orchestrates the construction steps
    /// </summary>
    public async Task<WorkoutPlanResponse> CreateBeginnerFullBodyAsync(
        int clientId,
        CancellationToken cancellationToken = default)
    {
        // Validate client exists
        var client = await _clientRepository.GetByIdWithDetailsAsync(clientId, cancellationToken);
        if (client == null)
            throw new InvalidOperationException($"Client with ID {clientId} not found");

        // Director constructs the workout plan
        _director.ConstructBeginnerFullBody(clientId);

        // Get product from builder
        var workoutPlan = _builder.Build();

        // Save to database
        var savedWorkoutPlan = await _workoutPlanRepository.AddAsync(workoutPlan, cancellationToken);

        // Reload with relationships
        var fullWorkoutPlan = await _workoutPlanRepository.GetByIdWithDetailsAsync(
            savedWorkoutPlan.Id,
            cancellationToken
        );

        return MapToResponse(fullWorkoutPlan!);
    }

    /// <summary>
    /// Creates an Intermediate Strength program using Director
    /// </summary>
    public async Task<WorkoutPlanResponse> CreateIntermediateStrengthAsync(
        int clientId,
        CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdWithDetailsAsync(clientId, cancellationToken);
        if (client == null)
            throw new InvalidOperationException($"Client with ID {clientId} not found");

        _director.ConstructIntermediateStrength(clientId);
        var workoutPlan = _builder.Build();

        var savedWorkoutPlan = await _workoutPlanRepository.AddAsync(workoutPlan, cancellationToken);
        var fullWorkoutPlan = await _workoutPlanRepository.GetByIdWithDetailsAsync(
            savedWorkoutPlan.Id,
            cancellationToken
        );

        return MapToResponse(fullWorkoutPlan!);
    }

    /// <summary>
    /// Creates an Advanced Muscle Gain program using Director
    /// </summary>
    public async Task<WorkoutPlanResponse> CreateAdvancedMuscleGainAsync(
        int clientId,
        CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdWithDetailsAsync(clientId, cancellationToken);
        if (client == null)
            throw new InvalidOperationException($"Client with ID {clientId} not found");

        _director.ConstructAdvancedMuscleGain(clientId);
        var workoutPlan = _builder.Build();

        var savedWorkoutPlan = await _workoutPlanRepository.AddAsync(workoutPlan, cancellationToken);
        var fullWorkoutPlan = await _workoutPlanRepository.GetByIdWithDetailsAsync(
            savedWorkoutPlan.Id,
            cancellationToken
        );

        return MapToResponse(fullWorkoutPlan!);
    }

    /// <summary>
    /// Creates a Weight Loss program using Director
    /// </summary>
    public async Task<WorkoutPlanResponse> CreateWeightLossProgramAsync(
        int clientId,
        CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdWithDetailsAsync(clientId, cancellationToken);
        if (client == null)
            throw new InvalidOperationException($"Client with ID {clientId} not found");

        _director.ConstructWeightLossProgram(clientId);
        var workoutPlan = _builder.Build();

        var savedWorkoutPlan = await _workoutPlanRepository.AddAsync(workoutPlan, cancellationToken);
        var fullWorkoutPlan = await _workoutPlanRepository.GetByIdWithDetailsAsync(
            savedWorkoutPlan.Id,
            cancellationToken
        );

        return MapToResponse(fullWorkoutPlan!);
    }

    /// <summary>
    /// Creates an Endurance program using Director
    /// </summary>
    public async Task<WorkoutPlanResponse> CreateEnduranceProgramAsync(
        int clientId,
        CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdWithDetailsAsync(clientId, cancellationToken);
        if (client == null)
            throw new InvalidOperationException($"Client with ID {clientId} not found");

        _director.ConstructEnduranceProgram(clientId);
        var workoutPlan = _builder.Build();

        var savedWorkoutPlan = await _workoutPlanRepository.AddAsync(workoutPlan, cancellationToken);
        var fullWorkoutPlan = await _workoutPlanRepository.GetByIdWithDetailsAsync(
            savedWorkoutPlan.Id,
            cancellationToken
        );

        return MapToResponse(fullWorkoutPlan!);
    }

    public async Task<List<WorkoutPlanResponse>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByUserIdAsync(userId, cancellationToken);
        if (client == null)
            throw new InvalidOperationException("Client profile not found for current user.");

        var plans = await _workoutPlanRepository.GetByClientIdAsync(client.Id, cancellationToken);
        return plans.Select(MapToResponse).ToList();
    }
}