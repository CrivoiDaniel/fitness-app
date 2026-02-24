using System;

namespace FitnessApp.Application.DTOs.Statistics;

public class ExpiringSubscriptionDto
{
    public int SubscriptionId { get; set; }
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string PlanType { get; set; } = string.Empty;
    public DateTime EndDate { get; set; }
    public int DaysRemaining { get; set; }
}