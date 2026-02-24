using System;

namespace FitnessApp.Application.DTOs.Workouts;

/// <summary>
/// Response DTO for Exercise within a workout plan
/// </summary>
public class ExerciseResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Sets { get; set; }
    public int Reps { get; set; }
    public int? DurationSeconds { get; set; }
    public int OrderInWorkout { get; set; }
    public string? Notes { get; set; }
}