using System;

namespace FitnessApp.Domain.Factories.Subscriptions.Products.PaymentPlans;

public class IndividualPaymentPlan : IPaymentPlan
{
    public bool SupportsInstallments() => false;
    public int GetDefaultInstallmentCount() => 1;
    public bool IsRecurring() => false;

}
