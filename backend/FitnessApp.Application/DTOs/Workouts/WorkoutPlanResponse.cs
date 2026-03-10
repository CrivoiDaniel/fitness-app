using System;

namespace FitnessApp.Application.DTOs.Workouts;

public class WorkoutPlanResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;

    public int? TrainerId { get; set; }
    public string? TrainerName { get; set; }

    public string Goal { get; set; } = string.Empty;
    public string Difficulty { get; set; } = string.Empty;
    public int DurationWeeks { get; set; }

    public List<string> WorkoutDays { get; set; } = new();
    public int SessionsPerWeek { get; set; }
    public int SessionDurationMinutes { get; set; }
    public int? RestDaysBetweenSessions { get; set; }

    public int TotalSessions { get; set; }
    public int TotalMinutes { get; set; }

    public List<ExerciseResponse> Exercises { get; set; } = new();

    public bool IsActive { get; set; }
    public string? SpecialNotes { get; set; }

    public DateTime CreatedAt { get; set; }

    // NEW (Composite)
    public int TotalSets { get; set; }
    public int TotalReps { get; set; }
    public int TotalExerciseDurationSeconds { get; set; }
}