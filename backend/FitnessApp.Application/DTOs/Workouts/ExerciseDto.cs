using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs.Workouts;

/// <summary>
/// DTO for exercise within a workout plan
/// </summary>
public class ExerciseDto
{
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Range(1, 100)]
    public int Sets { get; set; }
    
    [Required]
    [Range(1, 1000)]
    public int Reps { get; set; }
    
    [Range(1, 3600)]
    public int? DurationSeconds { get; set; }
    
    [StringLength(500)]
    public string? Notes { get; set; }
}