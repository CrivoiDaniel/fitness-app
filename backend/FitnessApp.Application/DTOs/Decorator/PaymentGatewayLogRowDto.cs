namespace FitnessApp.Application.DTOs.Decorator;

public class PaymentGatewayLogRowDto
{
    public int Id { get; set; }
    public string Provider { get; set; } = string.Empty;
    public int SubscriptionId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public int Attempt { get; set; }
    public bool IsSuccess { get; set; }
    public int DurationMs { get; set; }
    public string? TransactionId { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; }
}