using System;
using FitnessApp.Application.DTOs.Subscriptions.Purchase;

namespace FitnessApp.Application.Interfaces.Subscriptions;

public interface IPurchaseSubscriptionService
{
    Task<PurchaseSubscriptionResultDto> PurchaseAsync(int userId, PurchaseSubscriptionRequestDto dto, CancellationToken cancellationToken = default);
}
