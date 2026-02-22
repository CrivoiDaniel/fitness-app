using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs.Subscriptions.BenefitPackage;

public class CreateBenefitPackageItemDto
{
    [Required(ErrorMessage = "Benefit ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Benefit ID must be greater than 0.")]
    public int BenefitId { get; set; }

    [Required(ErrorMessage = "Value is required.")]
    [MaxLength(255, ErrorMessage = "Value must not exceed 255 characters.")]
    public string Value { get; set; } = string.Empty;
}
