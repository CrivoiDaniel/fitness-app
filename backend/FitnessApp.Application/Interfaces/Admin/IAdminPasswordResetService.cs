using System;
using FitnessApp.Application.DTOs.Admin;

namespace FitnessApp.Application.Interfaces.Admin;

public interface IAdminPasswordResetService
{
    Task<ResetPasswordResponseDto> ResetPasswordAsync(int userId);
}