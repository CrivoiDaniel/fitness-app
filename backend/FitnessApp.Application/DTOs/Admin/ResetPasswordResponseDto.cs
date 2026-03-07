using System;

namespace FitnessApp.Application.DTOs.Admin;

public class ResetPasswordResponseDto
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string TemporaryPassword { get; set; } = string.Empty;
    public string Message { get; set; } = "Password reset successfully";
}