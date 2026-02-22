using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs.Subscriptions.BenefitPackage;

public class CreateBenefitPackageDto
{
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(100, ErrorMessage = "Name must not exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Weekday schedule is required.")]
    [MaxLength(50, ErrorMessage = "Weekday schedule must not exceed 50 characters.")]
    public string ScheduleWeekday { get; set; } = string.Empty;

    [Required(ErrorMessage = "Weekend schedule is required.")]
    [MaxLength(50, ErrorMessage = "Weekend schedule must not exceed 50 characters.")]
    public string ScheduleWeekend { get; set; } = string.Empty;

    [Required(ErrorMessage = "At least one benefit item is required.")]
    [MinLength(1, ErrorMessage = "At least one benefit item is required.")]
    public List<CreateBenefitPackageItemDto> Items { get; set; } = new();
}