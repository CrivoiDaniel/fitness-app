using System;
using System.ComponentModel.DataAnnotations;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.DTOs.Workouts;

/// <summary>
/// Request DTO for creating a workout plan
/// </summary>
public class CreateWorkoutPlanRequest
{
    [Required]
    [StringLength(200, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int ClientId { get; set; }

    [Required]
    public WorkoutGoal Goal { get; set; }

    [Required]
    public DifficultyLevel Difficulty { get; set; }

    [Required]
    [Range(1, 52)]
    public int DurationWeeks { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one workout day must be specified")]
    public List<DayOfWeek> WorkoutDays { get; set; } = new();

    [Required]
    [Range(15, 240)]
    public int SessionDurationMinutes { get; set; }

    // Optional
    [Range(1, int.MaxValue)]
    public int? TrainerId { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [Range(0, 7)]
    public int? RestDays { get; set; }

    [StringLength(500)]
    public string? SpecialNotes { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "At least one exercise must be added")]
    public List<ExerciseDto> Exercises { get; set; } = new();
}