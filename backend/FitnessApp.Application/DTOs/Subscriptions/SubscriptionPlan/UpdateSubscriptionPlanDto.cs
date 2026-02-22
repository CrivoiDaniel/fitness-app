using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs.Subscriptions.SubscriptionPlan;

public class UpdateSubscriptionPlanDto
{
    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999999.99.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Benefit package ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Benefit package ID must be greater than 0.")]
    public int BenefitPackageId { get; set; }

    public bool IsRecurring { get; set; }

    public bool AllowInstallments { get; set; }

    [Range(1, 24, ErrorMessage = "Max installments must be between 1 and 24.")]
    public int MaxInstallments { get; set; }

    public bool IsActive { get; set; }
}