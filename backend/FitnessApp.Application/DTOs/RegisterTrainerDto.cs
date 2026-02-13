using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs;

public class RegisterTrainerDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    [MaxLength(100, ErrorMessage = "Email must be between 1 and 100 characters.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    [MaxLength(100, ErrorMessage = "Password must be between 1 and 100 characters.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required")]
    [MaxLength(100, ErrorMessage = "First name must be between 1 and 100 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [MaxLength(100, ErrorMessage = "Last name must be between 1 and 100 characters.")]
    public string LastName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number format")]
    [MaxLength(20, ErrorMessage = "Phone number must be between 1 and 20 characters.")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Specialization is required")]
    [MaxLength(100, ErrorMessage = "Specialization must be between 1 and 100 characters.")]
    public string Specialization { get; set; } = string.Empty;

    [Required(ErrorMessage = "Years of experience is required")]
    [Range(0, 100, ErrorMessage = "Years of experience must be between 0 and 100.")]
    public int YearsOfExperience { get; set; }
}
