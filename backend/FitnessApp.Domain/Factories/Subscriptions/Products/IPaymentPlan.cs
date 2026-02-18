using System;

namespace FitnessApp.Domain.Factories.Subscriptions.Products;

//Defines LOGIC for payment behavior
public interface IPaymentPlan
{
    bool SupportsInstallments();
    int GetDefaultInstallmentCount();
    bool IsRecurring();

}
