using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs.Subscriptions.Benefit;

public class CreateBenefitDto
{
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(100, ErrorMessage = "Name must not exceed 100 characters.")]
    [RegularExpression("^[a-z_]+$", ErrorMessage = "Name must contain only lowercase letters and underscores.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Display name is required.")]
    [MaxLength(255, ErrorMessage = "Display name must not exceed 255 characters.")]
    public string DisplayName { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Description must not exceed 1000 characters.")]
    public string? Description { get; set; }
}
