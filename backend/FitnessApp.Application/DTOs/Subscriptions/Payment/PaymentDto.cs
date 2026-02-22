using System;

namespace FitnessApp.Application.DTOs.Subscriptions.Payment;

public class PaymentDto
{
    public int Id { get; set; }
    public int SubscriptionId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public int InstallmentNumber { get; set; }
    public string? TransactionId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}