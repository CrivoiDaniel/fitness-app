using System;
using FitnessApp.Domain.Entities.Base;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Domain.Entities.Auth;


/// <summary>
/// Refresh Token stored in database
/// Used for secure token renewal and session management
/// </summary>
public class RefreshToken : BaseEntity
{
    public int UserId { get; private set; }
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public string? RevokedByIp { get; private set; }
    public string? ReplacedByToken { get; private set; }
    public string CreatedByIp { get; private set; } = string.Empty;
    
    // Navigation
    public virtual User User { get; set; } = null!;
    
    private RefreshToken() : base() { }
    
    public RefreshToken(
        int userId,
        string token,
        DateTime expiresAt,
        string createdByIp) : base()
    {
        if (userId <= 0)
            throw new ArgumentException("UserId must be positive", nameof(userId));
        
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token cannot be empty", nameof(token));
        
        if (expiresAt <= DateTime.UtcNow)
            throw new ArgumentException("ExpiresAt must be in the future", nameof(expiresAt));
        
        UserId = userId;
        Token = token;
        ExpiresAt = expiresAt;
        CreatedByIp = createdByIp ?? "unknown";
        IsRevoked = false;
    }
    
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;
    
    public void Revoke(string? revokedByIp = null, string? replacedByToken = null)
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
        RevokedByIp = revokedByIp;
        ReplacedByToken = replacedByToken;
    }
}