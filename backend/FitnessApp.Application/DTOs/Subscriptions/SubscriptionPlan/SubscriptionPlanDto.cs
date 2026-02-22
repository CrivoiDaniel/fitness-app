using System;
using FitnessApp.Application.DTOs.Subscriptions.BenefitPackage;

namespace FitnessApp.Application.DTOs.Subscriptions.SubscriptionPlan;

public class SubscriptionPlanDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public int DurationInMonths { get; set; }
    public decimal Price { get; set; }
    public int BenefitPackageId { get; set; }
    public string BenefitPackageName { get; set; } = string.Empty;
    public bool IsRecurring { get; set; }
    public bool AllowInstallments { get; set; }
    public int MaxInstallments { get; set; }
    public bool IsActive { get; set; }
    public BenefitPackageDto? BenefitPackage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
