using System;
using FitnessApp.Domain.Entities.Base;

namespace FitnessApp.Domain.Entities.Subscriptions;

//list of all available benefits
public class Benefit : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string DisplayName { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }

    //NAVIGATION
    public virtual ICollection<BenefitPackageItem> BenefitPackageItems { get; set; } = new List<BenefitPackageItem>();

    private Benefit() : base() { }

    public Benefit(
        string name,
        string displayName,
        string? description = null) : base()
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");

        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name cannot be empty");

        Name = name;
        DisplayName = displayName;
        Description = description;
        IsActive = true;
    }

    //public methods

    public void UpdateDisplayName(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name cannot be empty");

        DisplayName = displayName;
        UpdateTimestamp();
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
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

}
