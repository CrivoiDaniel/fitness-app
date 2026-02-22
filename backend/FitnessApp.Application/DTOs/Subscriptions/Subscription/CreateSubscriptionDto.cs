using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs.Subscriptions.Subscription;

public class CreateSubscriptionDto
{
    [Required(ErrorMessage = "Client ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Client ID must be greater than 0.")]
    public int ClientId { get; set; }

    [Required(ErrorMessage = "Subscription plan ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Subscription plan ID must be greater than 0.")]
    public int SubscriptionPlanId { get; set; }

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; }

    public bool AutoRenew { get; set; } = true;
}