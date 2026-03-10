using System;

namespace FitnessApp.Application.Payments.Gateways;
public interface IPaymentGateway
{
    Task<GatewayChargeResult> CreateChargeAsync(GatewayChargeRequest request, CancellationToken cancellationToken = default);
}