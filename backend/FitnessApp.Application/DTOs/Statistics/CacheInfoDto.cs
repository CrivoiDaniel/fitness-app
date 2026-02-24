using System;

namespace FitnessApp.Application.DTOs.Statistics;

public class CacheInfoDto
{
    public DateTime LastUpdate { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsExpired { get; set; }
    public int CachedSubscriptions { get; set; }
    public int CachedPayments { get; set; }
    public int CacheExpirationMinutes { get; set; }
}
