using System;

namespace FitnessApp.Infrastructure.Payments.Stripe;

public class StripeOptions
{
    public string SecretKey { get; set; } = string.Empty;
}
