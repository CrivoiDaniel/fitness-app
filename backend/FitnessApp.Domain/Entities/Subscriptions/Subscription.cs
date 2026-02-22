using System;
using FitnessApp.Domain.Entities.Base;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Domain.Entities.Subscriptions;

public class Subscription : BaseEntity
{
    public int ClientId { get; private set; }
    public int SubscriptionPlanId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public bool AutoRenew { get; private set; }

    // Navigation
    public virtual Client Client { get; set; } = null!;
    public virtual SubscriptionPlan SubscriptionPlan { get; set; } = null!;
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    private Subscription() : base() { }

    public Subscription(
        int clientId,
        int subscriptionPlanId,
        DateTime startDate,
        DateTime? endDate,
        bool autoRenew = true) : base()
    {
        if (clientId <= 0)
            throw new ArgumentException("Client ID must be positive");

        if (subscriptionPlanId <= 0)
            throw new ArgumentException("Subscription plan ID must be positive");

        if (endDate.HasValue && endDate.Value <= startDate)
            throw new ArgumentException("End date must be after start date");

        ClientId = clientId;
        SubscriptionPlanId = subscriptionPlanId;
        StartDate = startDate;
        EndDate = endDate;
        Status = SubscriptionStatus.Pending;
        AutoRenew = autoRenew;
    }

    // PUBLIC METHODS
    public void Activate()
    {
        if (Status == SubscriptionStatus.Cancelled)
            throw new InvalidOperationException("Cannot activate cancelled subscription");

        Status = SubscriptionStatus.Active;
    }

    public void Cancel()
    {
        if (Status == SubscriptionStatus.Cancelled)
            throw new InvalidOperationException("Subscription is already cancelled");

        Status = SubscriptionStatus.Cancelled;
        AutoRenew = false;
    }

    public void MarkAsExpired()
    {
        if (Status != SubscriptionStatus.Active)
            throw new InvalidOperationException("Only active subscriptions can expire");

        Status = SubscriptionStatus.Expired;
    }

    public void Renew(DateTime newEndDate)
    {
        if (newEndDate <= (EndDate ?? StartDate))
            throw new ArgumentException("New end date must be after current end date");

        EndDate = newEndDate;
        Status = SubscriptionStatus.Active;
    }

    public void Extend(int days)
    {
        if (days <= 0)
            throw new ArgumentException("Days must be positive");

        if (!EndDate.HasValue)
            throw new InvalidOperationException("Cannot extend subscription without end date");

        EndDate = EndDate.Value.AddDays(days);
    }

    public void SetAutoRenew(bool autoRenew)
    {
        AutoRenew = autoRenew;
    }

    // COMPUTED PROPERTIES
    public bool HasExpired()
    {
        if (!EndDate.HasValue)
            return false;

        return DateTime.UtcNow.Date > EndDate.Value.Date;
    }

    public int DaysRemaining()
    {
        if (!EndDate.HasValue || HasExpired())
            return 0;

        return (EndDate.Value.Date - DateTime.UtcNow.Date).Days;
    }

    public bool ExpiresSoon(int withinDays = 7)
    {
        int daysRemaining = DaysRemaining();
        return daysRemaining > 0 && daysRemaining <= withinDays;
    }

    public bool IsActive()
    {
        return Status == SubscriptionStatus.Active && !HasExpired();
    }

}
