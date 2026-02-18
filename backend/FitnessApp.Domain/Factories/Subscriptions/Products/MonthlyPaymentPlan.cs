using System;

namespace FitnessApp.Domain.Factories.Subscriptions.Products;

public class MonthlyPaymentPlan : IPaymentPlan
{
    public bool SupportsInstallments() => false;
    public int GetDefaultInstallmentCount() => 1;
    public bool IsRecurring() => true;

}
