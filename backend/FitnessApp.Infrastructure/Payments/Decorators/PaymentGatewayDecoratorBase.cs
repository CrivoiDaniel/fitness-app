using FitnessApp.Application.Payments.Gateways;

namespace FitnessApp.Infrastructure.Payments.Decorators;

public abstract class PaymentGatewayDecoratorBase : IPaymentGateway
{
    protected readonly IPaymentGateway Wrappee;

    protected PaymentGatewayDecoratorBase(IPaymentGateway wrappee)
    {
        Wrappee = wrappee;
    }

    public virtual Task<GatewayChargeResult> CreateChargeAsync(GatewayChargeRequest request, CancellationToken cancellationToken = default)
        => Wrappee.CreateChargeAsync(request, cancellationToken);
}