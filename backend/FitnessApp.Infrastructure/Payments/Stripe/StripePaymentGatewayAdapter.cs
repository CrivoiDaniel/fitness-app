using System;
using FitnessApp.Application.Payments.Gateways;
using Microsoft.Extensions.Options;
using Stripe;

namespace FitnessApp.Infrastructure.Payments.Stripe;

public class StripePaymentGatewayAdapter : IPaymentGateway
{
    private readonly StripeOptions _options;

    public StripePaymentGatewayAdapter(IOptions<StripeOptions> options)
    {
        _options = options.Value;

        if (string.IsNullOrWhiteSpace(_options.SecretKey))
            throw new InvalidOperationException("Stripe SecretKey is missing. Configure Stripe:SecretKey in appsettings.");
    }

    public async Task<GatewayChargeResult> CreateChargeAsync(GatewayChargeRequest request, CancellationToken cancellationToken = default)
    {
        StripeConfiguration.ApiKey = _options.SecretKey;

        // Stripe lucrează în "minor units" (cents). Pentru MDL: 1 leu = 100 bani.
        var amountMinor = (long)Math.Round(request.Amount * 100m, 0);

        var service = new PaymentIntentService();

        var metadata = new Dictionary<string, string>
        {
            ["subscriptionId"] = request.SubscriptionId.ToString(),
            ["installmentNumber"] = request.InstallmentNumber.ToString()
        };

        var intent = await service.CreateAsync(new PaymentIntentCreateOptions
        {
            Amount = amountMinor,
            Currency = request.Currency,     // ex: "mdl"
            Description = request.Description ?? $"Subscription #{request.SubscriptionId} payment",
            Metadata = metadata,

            // IMPORTANT: fără confirmare automată aici (confirmarea o poate face frontend Stripe.js)
            PaymentMethodTypes = new List<string> { "card" }
        }, cancellationToken: cancellationToken);

        return new GatewayChargeResult
        {
            IsCreated = true,
            Provider = "Stripe",
            TransactionId = intent.Id,      // pi_...
            ClientSecret = intent.ClientSecret,
            Status = intent.Status ?? "Pending",
            Raw = null
        };
    }
}
