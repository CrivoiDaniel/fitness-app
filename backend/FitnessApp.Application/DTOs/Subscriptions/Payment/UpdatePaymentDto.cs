using System;
using System.ComponentModel.DataAnnotations;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.DTOs.Subscriptions.Payment;

public class UpdatePaymentDto
{
    [Required(ErrorMessage = "Status is required.")]
    public PaymentStatus Status { get; set; }

    [MaxLength(255, ErrorMessage = "Transaction ID must not exceed 255 characters.")]
    public string? TransactionId { get; set; }
}