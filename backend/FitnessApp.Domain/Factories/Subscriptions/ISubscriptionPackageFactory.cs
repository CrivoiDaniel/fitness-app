using System;
using FitnessApp.Domain.Factories.Subscriptions.Products.BenefitsPackage;
using FitnessApp.Domain.Factories.Subscriptions.Products.PaymentPlans;
using FitnessApp.Domain.Factories.Subscriptions.Products.SubscriptionPlans;
namespace FitnessApp.Domain.Factories.Subscriptions;

//Abstract Factory - Creates family of related subscription products
public interface ISubscriptionPackageFactory
{
    ISubscriptionPlan CreatePlan();
    IPaymentPlan CreatePaymentPlan();
    IBenefitsPackage CreateBenefitsPackage();

}
