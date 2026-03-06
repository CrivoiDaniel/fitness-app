using FitnessApp.Application.DTOs.Auth;
using FitnessApp.Application.Interfaces.Auth;
using FitnessApp.Application.Interfaces.Repositories.Users;
using FitnessApp.Application.Settings;
using FitnessApp.Domain.Entities.Auth;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;


namespace FitnessApp.Application.Services.Auth;


public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly JwtSettings _jwtSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public AuthService(
        IUserRepository userRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService,
        IOptions<JwtSettings> jwtSettings,
        IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
        _jwtSettings = jwtSettings.Value;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        // 1. Find user by email
        var user = await _userRepository.GetByEmailAsync(request.Email);
        
        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password");
        
        // 2. Verify password
        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password");
        
        // 3. Check if user is active
        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is deactivated. Contact admin.");
        
        // 4. Generate JWT access token
        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        
        // 5. Generate refresh token
        var refreshTokenString = _jwtTokenService.GenerateRefreshToken();
        
        // 6. Get client IP
        var ipAddress = GetIpAddress();
        
        // 7. Create RefreshToken entity and save to DB
        var refreshToken = new RefreshToken(
            userId: user.Id,
            token: refreshTokenString,
            expiresAt: DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            createdByIp: ipAddress
        );
        
        await _refreshTokenRepository.AddAsync(refreshToken);
        
        // 8. Return response
        return new LoginResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            ClientId = user.ClientProfile?.Id,
            TrainerId = user.TrainerProfile?.Id
        };
    }
    
    public async Task<LoginResponseDto> RefreshTokenAsync(string refreshTokenString)
    {
        // 1. Get refresh token from DB
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshTokenString);
        
        if (refreshToken == null)
            throw new UnauthorizedAccessException("Invalid refresh token");
        
        // 2. Validate refresh token
        if (!refreshToken.IsActive)
            throw new UnauthorizedAccessException("Refresh token expired or revoked");
        
        // 3. Get user
        var user = await _userRepository.GetByIdAsync(refreshToken.UserId);
        
        if (user == null || !user.IsActive)
            throw new UnauthorizedAccessException("User not found or inactive");
        
        // 4. Generate new JWT access token
        var newAccessToken = _jwtTokenService.GenerateAccessToken(user);
        
        // 5. Generate new refresh token (rotation)
        var newRefreshTokenString = _jwtTokenService.GenerateRefreshToken();
        var ipAddress = GetIpAddress();
        
        // 6. Revoke old refresh token
        refreshToken.Revoke(ipAddress, newRefreshTokenString);
        await _refreshTokenRepository.UpdateAsync(refreshToken);
        
        // 7. Save new refresh token
        var newRefreshToken = new RefreshToken(
            userId: user.Id,
            token: newRefreshTokenString,
            expiresAt: DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            createdByIp: ipAddress
        );
        
        await _refreshTokenRepository.AddAsync(newRefreshToken);
        
        // 8. Return response
        return new LoginResponseDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role,
            ClientId = user.ClientProfile?.Id,
            TrainerId = user.TrainerProfile?.Id
        };
    }
    
    public async Task RevokeTokenAsync(string refreshTokenString)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(refreshTokenString);
        
        if (refreshToken == null)
            throw new InvalidOperationException("Token not found");
        
        if (!refreshToken.IsActive)
            throw new InvalidOperationException("Token already inactive");
        
        var ipAddress = GetIpAddress();
        refreshToken.Revoke(ipAddress);
        
        await _refreshTokenRepository.UpdateAsync(refreshToken);
    }
    
    public async Task LogoutAsync(int userId)
    {
        await _refreshTokenRepository.RevokeAllUserTokensAsync(userId);
    }
    
    private string GetIpAddress()
    {
        if (_httpContextAccessor.HttpContext?.Request.Headers.ContainsKey("X-Forwarded-For") == true)
        {
            return _httpContextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].ToString();
        }
        
        return _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}