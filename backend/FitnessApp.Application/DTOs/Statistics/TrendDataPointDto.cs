using System;

namespace FitnessApp.Application.DTOs.Statistics;

public class TrendDataPointDto
{
    public string Period { get; set; } = string.Empty;
    public int NewSubscriptions { get; set; }
    public int CancelledSubscriptions { get; set; }
    public int NetGrowth { get; set; }
}