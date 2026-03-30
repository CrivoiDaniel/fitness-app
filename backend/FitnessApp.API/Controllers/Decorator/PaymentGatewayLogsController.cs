using FitnessApp.Application.DTOs.Decorator;
using FitnessApp.Infrastructure.Repositories.Decorator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers.Decorator;


[ApiController]
[Route("api/admin/payment-gateway-logs")]
[Authorize(Roles = "Admin")]
public class PaymentGatewayLogsController : ControllerBase
{
    private readonly IPaymentGatewayLogRepository _repo;

    public PaymentGatewayLogsController(IPaymentGatewayLogRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult<List<PaymentGatewayLogRowDto>>> GetLatest([FromQuery] int take = 100,
        CancellationToken cancellationToken = default)
    {
        var logs = await _repo.GetLatestAsync(take, cancellationToken);

        var result = logs.Select(x => new PaymentGatewayLogRowDto
        {
            Id = x.Id,
            Provider = x.Provider,
            SubscriptionId = x.SubscriptionId,
            Amount = x.Amount,
            Currency = x.Currency,
            Attempt = x.Attempt,
            IsSuccess = x.IsSuccess,
            DurationMs = x.DurationMs,
            TransactionId = x.TransactionId,
            ErrorMessage = x.ErrorMessage,
            CreatedAt = x.CreatedAt
        }).ToList();

        return Ok(new { count = result.Count, first = result.FirstOrDefault(), data = result });
    }
}