using System;

namespace FitnessApp.Application.DTOs.Admin;

public class CreateUserResponseDto
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string TemporaryPassword { get; set; } = string.Empty;
    public string Message { get; set; } = "User created successfully";
}
