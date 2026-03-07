using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs.Admin;

public class UpdateSubscriptionPlanDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    [Range(0.01, 10000)]
    public decimal Price { get; set; }
    
    [Required]
    [Range(1, 365)]
    public int DurationDays { get; set; }
    
    [Required]
    public int BenefitPackageId { get; set; }
    
    public bool IsPopular { get; set; }
    public bool IsActive { get; set; }
}