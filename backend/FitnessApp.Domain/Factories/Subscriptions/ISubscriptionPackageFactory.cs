using System;
using FitnessApp.Domain.Factories.Subscriptions.Products;

namespace FitnessApp.Domain.Factories.Subscriptions;

//Abstract Factory - Creates family of related subscription products
public interface ISubscriptionPackageFactory
{
    ISubscriptionPlan CreatePlan();
    IPaymentPlan CreatePaymentPlan();
    IBenefitsPackage CreateBenefitsPackage();

}
