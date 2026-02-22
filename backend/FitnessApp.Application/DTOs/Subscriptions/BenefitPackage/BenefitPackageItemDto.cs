using System;

namespace FitnessApp.Application.DTOs.Subscriptions.BenefitPackage;

public class BenefitPackageItemDto
{
    public int BenefitId { get; set; }
    public string BenefitName { get; set; } = string.Empty;
    public string BenefitDisplayName { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
