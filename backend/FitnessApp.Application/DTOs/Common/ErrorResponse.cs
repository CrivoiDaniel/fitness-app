using System;

namespace FitnessApp.Application.DTOs.Common;


/// <summary>
/// Standard error response for API
/// </summary>
public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public string? Details { get; set; }
    public DateTime Timestamp { get; set; }
}