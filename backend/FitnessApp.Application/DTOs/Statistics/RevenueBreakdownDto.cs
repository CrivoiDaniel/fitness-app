using System;

namespace FitnessApp.Application.DTOs.Statistics;

public class RevenueBreakdownDto
{
    public decimal Today { get; set; }
    public decimal ThisWeek { get; set; }
    public decimal ThisMonth { get; set; }
    public decimal ThisYear { get; set; }
    public decimal AllTime { get; set; }
}