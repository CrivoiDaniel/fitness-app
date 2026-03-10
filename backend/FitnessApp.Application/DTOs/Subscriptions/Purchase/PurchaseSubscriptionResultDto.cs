using System;

namespace FitnessApp.Application.DTOs.Subscriptions.Purchase;

public class PurchaseSubscriptionResultDto
{
    public int SubscriptionId { get; set; }
    public int PaymentId { get; set; }
    public string PaymentStatus { get; set; } = "Pending";
    public string? TransactionId { get; set; }

    // Stripe optional (dacă decizi să-l expui)
    public string? ClientSecret { get; set; }

    public string Provider { get; set; } = string.Empty;
}
