using System.Diagnostics;
using FitnessApp.Application.Payments.Gateways;
using FitnessApp.Domain.Decorator;
using FitnessApp.Infrastructure.Repositories.Decorator;

namespace FitnessApp.Infrastructure.Payments.Decorators;

public sealed class DbLoggingPaymentGatewayDecorator : PaymentGatewayDecoratorBase
{
    private readonly IPaymentGatewayLogRepository _logRepository;

    public DbLoggingPaymentGatewayDecorator(
        IPaymentGateway wrappee,
        IPaymentGatewayLogRepository logRepository) : base(wrappee)
    {
        _logRepository = logRepository;
    }

    public override async Task<GatewayChargeResult> CreateChargeAsync(GatewayChargeRequest request, CancellationToken cancellationToken = default)
    {
        var sw = Stopwatch.StartNew();
        try
        {
            var result = await base.CreateChargeAsync(request, cancellationToken);
            sw.Stop();

            await _logRepository.AddAsync(new PaymentGatewayLog(
                provider: result.Provider,
                subscriptionId: request.SubscriptionId,
                amount: request.Amount,
                currency: request.Currency,
                attempt: 1, // NOTE: retry decorator va gestiona attempt; vezi mai jos
                isSuccess: result.IsCreated,
                durationMs: (int)sw.ElapsedMilliseconds,
                transactionId: result.TransactionId,
                errorMessage: null
            ), cancellationToken);

            return result;
        }
        catch (Exception ex)
        {
            sw.Stop();

            await _logRepository.AddAsync(new PaymentGatewayLog(
                provider: "Unknown",
                subscriptionId: request.SubscriptionId,
                amount: request.Amount,
                currency: request.Currency,
                attempt: 1,
                isSuccess: false,
                durationMs: (int)sw.ElapsedMilliseconds,
                transactionId: null,
                errorMessage: ex.Message
            ), cancellationToken);

            throw;
        }
    }
}