using System;

namespace FitnessApp.Application.Payments.Gateways;

public class GatewayChargeRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "mdl"; // Stripe recomandă lowercase ISO code
    public string? Description { get; set; }

    // Pentru corelare (debug + audit)
    public int SubscriptionId { get; set; }
    public int InstallmentNumber { get; set; } = 1;
}