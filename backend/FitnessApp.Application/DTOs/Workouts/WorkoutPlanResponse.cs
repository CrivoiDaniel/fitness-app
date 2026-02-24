using System;

namespace FitnessApp.Application.DTOs.Workouts;

/// <summary>
/// Response DTO for WorkoutPlan
/// </summary>
public class WorkoutPlanResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Client info
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;

    // Trainer info (optional)
    public int? TrainerId { get; set; }
    public string? TrainerName { get; set; }

    // Configuration
    public string Goal { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public int DurationWeeks { get; set; }

    // Schedule
    public List<string> WorkoutDays { get; set; } = new();
    public int SessionsPerWeek { get; set; }
    public int SessionDurationMinutes { get; set; }
    public int? RestDaysBetweenSessions { get; set; }

    // Computed
    public int TotalSessions { get; set; }
    public int TotalMinutes { get; set; }

    // Exercises
    public List<ExerciseResponse> Exercises { get; set; } = new();

    // Status
    public bool IsActive { get; set; }
    public string? SpecialNotes { get; set; }

    // Metadata
    public DateTime CreatedAt { get; set; }
}