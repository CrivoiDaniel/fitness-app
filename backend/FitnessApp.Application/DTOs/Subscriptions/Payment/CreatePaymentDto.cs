using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs.Subscriptions.Payment;

public class CreatePaymentDto
{
    [Required(ErrorMessage = "Subscription ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Subscription ID must be greater than 0.")]
    public int SubscriptionId { get; set; }

    [Required(ErrorMessage = "Amount is required.")]
    [Range(0.01, 999999.99, ErrorMessage = "Amount must be between 0.01 and 999999.99.")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "Payment date is required.")]
    public DateTime PaymentDate { get; set; }

    [Range(1, 100, ErrorMessage = "Installment number must be between 1 and 100.")]
    public int InstallmentNumber { get; set; } = 1;

    [MaxLength(255, ErrorMessage = "Transaction ID must not exceed 255 characters.")]
    public string? TransactionId { get; set; }
}