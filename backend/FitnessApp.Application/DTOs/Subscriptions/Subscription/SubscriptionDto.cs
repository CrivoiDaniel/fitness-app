using System;
using FitnessApp.Application.DTOs.Subscriptions.Payment;
using FitnessApp.Application.DTOs.Subscriptions.SubscriptionPlan;

namespace FitnessApp.Application.DTOs.Subscriptions.Subscription;

public class SubscriptionDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public int SubscriptionPlanId { get; set; }
    public string PlanType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool AutoRenew { get; set; }
    public SubscriptionPlanDto? SubscriptionPlan { get; set; }
    public List<PaymentDto> Payments { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}