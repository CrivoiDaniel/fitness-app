using System;
using FitnessApp.Domain.Entities.Base;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Domain.Entities.Subscriptions;

public class Payment : BaseEntity
{
    public int SubscriptionId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime PaymentDate { get; private set; }
    public PaymentStatus Status { get; private set; }
    public int InstallmentNumber { get; private set; }
    public string? TransactionId { get; private set; }

    // Navigation
    public virtual Subscription Subscription { get; set; } = null!;

    private Payment() : base() { }

    public Payment(
        int subscriptionId,
        decimal amount,
        DateTime paymentDate,
        int installmentNumber = 1,
        string? transactionId = null) : base()
    {
        if (subscriptionId <= 0)
            throw new ArgumentException("Subscription ID must be positive");

        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");

        if (installmentNumber <= 0)
            throw new ArgumentException("Installment number must be positive");

        SubscriptionId = subscriptionId;
        Amount = amount;
        PaymentDate = paymentDate;
        Status = PaymentStatus.Pending;
        InstallmentNumber = installmentNumber;
        TransactionId = transactionId;
    }

    // PUBLIC METHODS
    public void MarkAsSuccess(string? transactionId = null)
    {
        if (Status == PaymentStatus.Success)
            throw new InvalidOperationException("Payment is already successful");

        Status = PaymentStatus.Success;

        if (!string.IsNullOrWhiteSpace(transactionId))
            TransactionId = transactionId;

        UpdateTimestamp();
    }

    public void MarkAsFailed()
    {
        if (Status == PaymentStatus.Success)
            throw new InvalidOperationException("Cannot mark successful payment as failed");

        Status = PaymentStatus.Failed;
        UpdateTimestamp();
    }

    public void UpdateTransactionId(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
            throw new ArgumentException("Transaction ID cannot be empty");

        TransactionId = transactionId;
        UpdateTimestamp();
    }

    public bool IsSuccessful()
    {
        return Status == PaymentStatus.Success;
    }

}
