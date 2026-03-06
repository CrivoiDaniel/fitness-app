using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs.Auth;

public class RefreshTokenRequestDto
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}