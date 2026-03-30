using FitnessApp.Domain.Entities.Base;

namespace FitnessApp.Domain.Decorator;

public class PaymentGatewayLog : BaseEntity
{
    public string Provider { get; private set; } = string.Empty;
    public int SubscriptionId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = string.Empty;

    public int Attempt { get; private set; }
    public bool IsSuccess { get; private set; }
    public int DurationMs { get; private set; }

    public string? TransactionId { get; private set; }
    public string? ErrorMessage { get; private set; }

    private PaymentGatewayLog() : base() { }

    public PaymentGatewayLog(
        string provider,
        int subscriptionId,
        decimal amount,
        string currency,
        int attempt,
        bool isSuccess,
        int durationMs,
        string? transactionId,
        string? errorMessage)
    {
        Provider = provider;
        SubscriptionId = subscriptionId;
        Amount = amount;
        Currency = currency;
        Attempt = attempt;
        IsSuccess = isSuccess;
        DurationMs = durationMs;
        TransactionId = transactionId;
        ErrorMessage = errorMessage;
    }
}