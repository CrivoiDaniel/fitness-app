using System;
using System.Security.Claims;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.Interfaces.Auth;

public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? ValidateToken(string token);
}