using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs.Subscriptions.Benefit;

public class UpdateBenefitDto
{
    [Required(ErrorMessage = "Display name is required.")]
    [MaxLength(255, ErrorMessage = "Display name must not exceed 255 characters.")]
    public string DisplayName { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Description must not exceed 1000 characters.")]
    public string? Description { get; set; }

    public bool IsActive { get; set; }
}
