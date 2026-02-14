using System;
using FitnessApp.Application.DTOs.Authentication;

namespace FitnessApp.Application.Interfaces.Authentication;

public interface IAuthenticationService
{
    //{/* Authentication */}
    Task<LoginResponseDto> LoginAsync(LoginDto loginDto);

    //{/* Validate Token */}
    Task<bool> ValidateTokenAsync(string token);
}
