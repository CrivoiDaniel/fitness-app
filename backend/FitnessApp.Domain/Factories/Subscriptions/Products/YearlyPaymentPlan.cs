using System;

namespace FitnessApp.Domain.Factories.Subscriptions.Products;

public class YearlyPaymentPlan : IPaymentPlan
{
    public bool SupportsInstallments() => true;
    public int GetDefaultInstallmentCount() => 12;
    public bool IsRecurring() => false;

}
