using FitnessApp.Application.Payments.Gateways;

namespace FitnessApp.Infrastructure.Payments.Decorators;

public sealed class RetryPaymentGatewayDecorator : PaymentGatewayDecoratorBase
{
    private readonly int _maxAttempts;
    private readonly TimeSpan _delay;

    public RetryPaymentGatewayDecorator(IPaymentGateway wrappee, int maxAttempts = 3, int delayMs = 250)
        : base(wrappee)
    {
        _maxAttempts = Math.Max(1, maxAttempts);
        _delay = TimeSpan.FromMilliseconds(Math.Max(0, delayMs));
    }

    public override async Task<GatewayChargeResult> CreateChargeAsync(GatewayChargeRequest request, CancellationToken cancellationToken = default)
    {
        Exception? last = null;

        for (var attempt = 1; attempt <= _maxAttempts; attempt++)
        {
            try
            {
                return await base.CreateChargeAsync(request, cancellationToken);
            }
            catch (OperationCanceledException) { throw; }
            catch (Exception ex)
            {
                Console.WriteLine($"[Stripe Error] Attempt {attempt} failed: {ex.Message}");
                last = ex;
                if (attempt == _maxAttempts) break;

                if (_delay > TimeSpan.Zero)
                    await Task.Delay(_delay, cancellationToken);
            }
        }

        throw last ?? new InvalidOperationException("Retry failed without exception.");
    }
}