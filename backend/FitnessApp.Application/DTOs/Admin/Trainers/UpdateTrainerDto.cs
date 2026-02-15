using System;

namespace FitnessApp.Application.DTOs.Admin.Trainers;

public class UpdateTrainerDto
{
    public string? Email {get; set;}
    public string? FirstName {get; set;}
    public string? LastName {get; set;}
    public string? PhoneNumber {get; set;}
    public bool? IsActive {get; set;}
    public string? Specialization {get; set;}
    public int? YearsOfExperience {get; set;}
}
