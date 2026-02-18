using System;
using FitnessApp.Domain.Entities.Base;

namespace FitnessApp.Domain.Entities.Subscriptions;

//Links benefits to packages with specific values
public class BenefitPackageItem : BaseEntity
{
    public int BenefitPackageId { get; private set; }
    public int BenefitId { get; private set; }
    public string Value { get; private set; } = string.Empty;

    // Navigation
    public virtual BenefitPackage BenefitPackage { get; set; } = null!;
    public virtual Benefit Benefit { get; set; } = null!;

    private BenefitPackageItem() : base() { }

    public BenefitPackageItem(
        int benefitPackageId,
        int benefitId,
        string value) : base()
    {
        if (benefitPackageId <= 0)
            throw new ArgumentException("Benefit package ID must be positive");

        if (benefitId <= 0)
            throw new ArgumentException("Benefit ID must be positive");

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty");

        BenefitPackageId = benefitPackageId;
        BenefitId = benefitId;
        Value = value;
    }

    // PUBLIC METHODS
    public void UpdateValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Value cannot be empty");

        Value = value;
        UpdateTimestamp();
    }

    // HELPER METHODS
    public bool GetBooleanValue()
    {
        return Value.Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    public int GetIntegerValue()
    {
        if (Value.Equals("unlimited", StringComparison.OrdinalIgnoreCase))
            return -1;

        return int.TryParse(Value, out int result) ? result : 0;
    }

    public bool IsUnlimited()
    {
        return Value.Equals("unlimited", StringComparison.OrdinalIgnoreCase);
    }

}
