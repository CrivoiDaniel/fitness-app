using System;
using FitnessApp.Application.Payments.Gateways;

namespace FitnessApp.Infrastructure.Payments.Paypal;

public class PaypalPaymentGatewayAdapter : IPaymentGateway
{
    public Task<GatewayChargeResult> CreateChargeAsync(GatewayChargeRequest request, CancellationToken cancellationToken = default)
    {
        // DEMO: generăm un order id fake
        var orderId = $"PAYPAL-DEMO-{Guid.NewGuid():N}".ToUpperInvariant();

        return Task.FromResult(new GatewayChargeResult
        {
            IsCreated = true,
            Provider = "PayPal(Demo)",
            TransactionId = orderId,
            ClientSecret = null,
            Status = "Pending",
            Raw = null
        });
    }
}