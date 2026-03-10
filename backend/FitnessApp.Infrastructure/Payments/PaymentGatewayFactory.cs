using System;
using FitnessApp.Application.Payments.Gateways;
using FitnessApp.Infrastructure.Payments.Paypal;
using FitnessApp.Infrastructure.Payments.Stripe;
using Microsoft.Extensions.Options;

namespace FitnessApp.Infrastructure.Payments;

public class PaymentGatewayFactory : IPaymentGatewayFactory
{
    private readonly PaymentGatewayOptions _options;
    private readonly StripePaymentGatewayAdapter _stripe;
    private readonly PaypalPaymentGatewayAdapter _paypal;

    public PaymentGatewayFactory(
        IOptions<PaymentGatewayOptions> options,
        StripePaymentGatewayAdapter stripe,
        PaypalPaymentGatewayAdapter paypal)
    {
        _options = options.Value;
        _stripe = stripe;
        _paypal = paypal;
    }

    public string DefaultCurrency => string.IsNullOrWhiteSpace(_options.Currency) ? "mdl" : _options.Currency;

    public IPaymentGateway GetGateway() => GetGateway(_options.Provider);

    public IPaymentGateway GetGateway(string provider)
    {
        var p = (provider ?? "Stripe").Trim();

        return p.Equals("Stripe", StringComparison.OrdinalIgnoreCase)
            ? _stripe
            : _paypal;
    }
}