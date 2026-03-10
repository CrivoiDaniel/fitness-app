using System;

namespace FitnessApp.Infrastructure.Payments;

public class PaymentGatewayOptions
{
    // "Stripe" | "PayPal"
    public string Provider { get; set; } = "Stripe";

    // currency default (pentru request)
    public string Currency { get; set; } = "mdl";
}
