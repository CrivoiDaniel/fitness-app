using System;

namespace FitnessApp.Application.DTOs.Subscriptions.BenefitPackage;

public class BenefitPackageDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ScheduleWeekday { get; set; } = string.Empty;
    public string ScheduleWeekend { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<BenefitPackageItemDto> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
