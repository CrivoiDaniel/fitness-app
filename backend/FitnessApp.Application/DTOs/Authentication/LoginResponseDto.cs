using System;

namespace FitnessApp.Application.DTOs.Authentication;

public class LoginResponseDto
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;

    public string Message { get; set; } = "Login successful";
}
