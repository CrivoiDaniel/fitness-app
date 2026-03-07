using System;
using FitnessApp.Application.DTOs.Auth;

namespace FitnessApp.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    Task<LoginResponseDto> RefreshTokenAsync(string refreshToken);
    Task RevokeTokenAsync(string refreshToken);
    Task LogoutAsync(int userId);

    Task ChangePasswordAsync(int userId, ChangePasswordRequestDto dto);
}