using System;

namespace FitnessApp.Domain.Factories.Subscriptions.Products.PaymentPlans;

public class StudentFitPaymentPlan : IPaymentPlan
{
    public bool SupportsInstallments() => false;
    public int GetDefaultInstallmentCount() => 1;
    public bool IsRecurring() => true;

}
