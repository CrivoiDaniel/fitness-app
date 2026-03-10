using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs.Subscriptions.Purchase;

public class PurchaseSubscriptionRequestDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int SubscriptionPlanId { get; set; }

    public bool AutoRenew { get; set; } = true;

    // NEW: ca să se vadă Adapter-ul (Stripe vs PayPal demo)
    [Required]
    public string Provider { get; set; } = "Stripe"; // "Stripe" | "PayPal"
}