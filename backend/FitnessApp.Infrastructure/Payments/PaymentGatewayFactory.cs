using System;
using FitnessApp.Application.Payments.Gateways;
using FitnessApp.Infrastructure.Payments.Decorators;
using FitnessApp.Infrastructure.Payments.Paypal;
using FitnessApp.Infrastructure.Payments.Stripe;
using FitnessApp.Infrastructure.Repositories.Decorator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FitnessApp.Infrastructure.Payments;

public class PaymentGatewayFactory : IPaymentGatewayFactory
{
    private readonly PaymentGatewayOptions _options;
    private readonly IServiceProvider _sp;
    private readonly IPaymentGatewayLogRepository _logRepo;

    public PaymentGatewayFactory(
        IOptions<PaymentGatewayOptions> options,
        IServiceProvider sp,
        IPaymentGatewayLogRepository logRepo)
    {
        _options = options.Value;
        _sp = sp;
        _logRepo = logRepo;
    }

    public string DefaultCurrency => string.IsNullOrWhiteSpace(_options.Currency) ? "mdl" : _options.Currency;

    public IPaymentGateway GetGateway() => GetGateway(_options.Provider);

    public IPaymentGateway GetGateway(string provider)
    {
        var p = (provider ?? "Stripe").Trim();

        IPaymentGateway baseGateway =
            p.Equals("Stripe", StringComparison.OrdinalIgnoreCase)
                ? _sp.GetRequiredService<StripePaymentGatewayAdapter>()
                : _sp.GetRequiredService<PaypalPaymentGatewayAdapter>();

        return new RetryPaymentGatewayDecorator(
            new DbLoggingPaymentGatewayDecorator(baseGateway, _logRepo),
            maxAttempts: 3,
            delayMs: 250
        );
    }
}