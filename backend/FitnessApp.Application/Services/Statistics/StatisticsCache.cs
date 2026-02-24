using System;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Interfaces.Statistics;

namespace FitnessApp.Application.Services.Statistics;


public class StatisticsCache : IStatisticsCache
{
    private readonly object _lock = new object();
    private List<Subscription> _subscriptions;
    private List<Payment> _payments;
    private DateTime _lastUpdate;
    private readonly TimeSpan _expirationTime;
    
    public StatisticsCache(TimeSpan expirationTime)
    {
        _subscriptions = new List<Subscription>();
        _payments = new List<Payment>();
        _lastUpdate = DateTime.MinValue;
        _expirationTime = expirationTime;
    }
    
    public void UpdateCache(IEnumerable<Subscription> subscriptions, IEnumerable<Payment> payments)
    {
        lock (_lock)
        {
            _subscriptions = subscriptions.ToList();
            _payments = payments.ToList();
            _lastUpdate = DateTime.UtcNow;
        }
    }
    
    public bool IsExpired()
    {
        lock (_lock)
        {
            return DateTime.UtcNow - _lastUpdate > _expirationTime;
        }
    }
    
    public void Clear()
    {
        lock (_lock)
        {
            _subscriptions.Clear();
            _payments.Clear();
            _lastUpdate = DateTime.MinValue;
        }
    }
    
    public IEnumerable<Subscription> GetCachedSubscriptions()
    {
        lock (_lock)
        {
            return _subscriptions.ToList();
        }
    }
    
    public IEnumerable<Payment> GetCachedPayments()
    {
        lock (_lock)
        {
            return _payments.ToList();
        }
    }
}