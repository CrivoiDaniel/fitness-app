using System;

namespace FitnessApp.Application.Payments.Gateways;

public class GatewayChargeResult
{
    public bool IsCreated { get; set; }
    public string Provider { get; set; } = string.Empty;

    // Exemplu: Stripe -> PaymentIntentId (pi_...)
    // PayPal -> OrderId
    public string TransactionId { get; set; } = string.Empty;

    // Pentru Stripe (dacă vrei confirmare din frontend)
    public string? ClientSecret { get; set; }

    public string Status { get; set; } = "Pending"; // "Pending" / "Succeeded" etc.
    public string? Raw { get; set; } // pentru debug (optional)
}