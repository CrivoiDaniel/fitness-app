using System;
using System.ComponentModel.DataAnnotations;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.DTOs.Subscriptions.Subscription;

public class UpdateSubscriptionDto
{
    public DateTime? EndDate { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public SubscriptionStatus Status { get; set; }

    public bool AutoRenew { get; set; }
}