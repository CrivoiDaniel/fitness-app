using System;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs.Admin;

public class CreateTrainerDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Invalid phone number.")]
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "Specialization is required.")]
    [MaxLength(100)]
    public string Specialization { get; set; } = string.Empty;

    [Required(ErrorMessage = "Years of experience is required.")]
    [Range(0, 50, ErrorMessage = "Years of experience must be between 0 and 50.")]
    public int YearsOfExperience { get; set; }

    //password is generated and sent to trainer via email

}