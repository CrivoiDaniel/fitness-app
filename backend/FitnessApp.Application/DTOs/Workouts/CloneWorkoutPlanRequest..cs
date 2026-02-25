using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs.Workouts;

/// <summary>
/// Request DTO for cloning a workout plan
/// </summary>
public class CloneWorkoutPlanRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int SourceWorkoutPlanId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue)]
    public int TargetClientId { get; set; }
    
    [StringLength(200)]
    public string? NewName { get; set; }
    
    [Range(1, int.MaxValue)]
    public int? NewTrainerId { get; set; }
}