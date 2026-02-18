using System;
using FitnessApp.Domain.Entities.Base;

namespace FitnessApp.Domain.Entities.Subscriptions;

//Reusable package of benefits with access schedule
public class BenefitPackage : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string ScheduleWeekday { get; private set; } = string.Empty;
    public string ScheduleWeekend { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }

    //navigation
    public virtual ICollection<BenefitPackageItem> BenefitPackageItems { get; set; } = new List<BenefitPackageItem>();
    public virtual ICollection<SubscriptionPlan> SubscriptionPlans { get; set; } = new List<SubscriptionPlan>();

    private BenefitPackage() : base() { }

    public BenefitPackage(
        string name,
        string scheduleWeekday,
        string scheduleWeekend) : base()
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");

        if (string.IsNullOrWhiteSpace(scheduleWeekday))
            throw new ArgumentException("Weekday schedule cannot be empty");

        if (string.IsNullOrWhiteSpace(scheduleWeekend))
            throw new ArgumentException("Weekend schedule cannot be empty");

        Name = name;
        ScheduleWeekday = scheduleWeekday;
        ScheduleWeekend = scheduleWeekend;
        IsActive = true;
    }

    // PUBLIC METHODS
    public void UpdateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty");

        Name = name;
        UpdateTimestamp();
    }

    public void UpdateSchedule(string scheduleWeekday, string scheduleWeekend)
    {
        if (string.IsNullOrWhiteSpace(scheduleWeekday))
            throw new ArgumentException("Weekday schedule cannot be empty");

        if (string.IsNullOrWhiteSpace(scheduleWeekend))
            throw new ArgumentException("Weekend schedule cannot be empty");

        ScheduleWeekday = scheduleWeekday;
        ScheduleWeekend = scheduleWeekend;
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

    public string GetFullSchedule()
    {
        return $"Monday-Friday: {ScheduleWeekday} | Saturday-Sunday: {ScheduleWeekend}";
    }

}
