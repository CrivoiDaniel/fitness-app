using System;
using FitnessApp.Domain.Entities.Base;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Domain.Entities.Subscriptions;

//Plans created by admin with pricing and benefit packages
public class SubscriptionPlan : BaseEntity
{
    public SubscriptionType Type { get; private set; }
    public int DurationInMonths { get; private set; }
    public decimal Price { get; private set; }
    public int BenefitPackageId { get; private set; }
    public bool IsRecurring { get; private set; }
    public bool AllowInstallments { get; private set; }
    public int MaxInstallments { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation
    public virtual BenefitPackage BenefitPackage { get; set; } = null!;
    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    private SubscriptionPlan() : base() { }

    public SubscriptionPlan(
        SubscriptionType type,
        int durationInMonths,
        decimal price,
        int benefitPackageId,
        bool isRecurring,
        bool allowInstallments,
        int maxInstallments) : base()
    {
        if (durationInMonths < 0)
            throw new ArgumentException("Duration cannot be negative");

        if (price < 0)
            throw new ArgumentException("Price cannot be negative");

        if (benefitPackageId <= 0)
            throw new ArgumentException("Benefit package ID must be positive");

        if (maxInstallments <= 0)
            throw new ArgumentException("Max installments must be positive");

        Type = type;
        DurationInMonths = durationInMonths;
        Price = price;
        BenefitPackageId = benefitPackageId;
        IsRecurring = isRecurring;
        AllowInstallments = allowInstallments;
        MaxInstallments = maxInstallments;
        IsActive = true;
    }

    // PUBLIC METHODS
    public void UpdatePrice(decimal price)
    {
        if (price < 0)
            throw new ArgumentException("Price cannot be negative");

        Price = price;
        UpdateTimestamp();
    }

    public void UpdateDuration(int durationInMonths)
    {
        if (durationInMonths < 0)
            throw new ArgumentException("Duration cannot be negative");

        DurationInMonths = durationInMonths;
        UpdateTimestamp();
    }

    public void UpdateInstallmentSettings(bool allowInstallments, int maxInstallments)
    {
        if (maxInstallments <= 0)
            throw new ArgumentException("Max installments must be positive");

        AllowInstallments = allowInstallments;
        MaxInstallments = maxInstallments;
        UpdateTimestamp();
    }

    public void ChangeBenefitPackage(int benefitPackageId)
    {
        if (benefitPackageId <= 0)
            throw new ArgumentException("Benefit package ID must be positive");

        BenefitPackageId = benefitPackageId;
        UpdateTimestamp();
    }

    public void Activate()
    {
        IsActive = true;
        UpdateTimestamp();
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }

    // COMPUTED PROPERTIES
    public decimal GetPricePerMonth()
    {
        if (DurationInMonths == 0)
            return Price;

        return Price / DurationInMonths;
    }

    public decimal GetInstallmentAmount()
    {
        if (!AllowInstallments || MaxInstallments == 0)
            return Price;

        return Price / MaxInstallments;
    }

    public bool HasFixedDuration()
    {
        return DurationInMonths > 0;
    }


}
