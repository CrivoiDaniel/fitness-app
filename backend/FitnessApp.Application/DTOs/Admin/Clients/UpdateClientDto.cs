using System;

namespace FitnessApp.Application.DTOs.Admin.Clients;

public class UpdateClientDto
{
    public string? Email {get; set;}
    public string? FirstName {get; set;}
    public string? LastName {get; set;}
    public string? PhoneNumber {get; set;}
    public bool? IsActive {get; set;}
    public DateTime? DateOfBirth {get; set;}
}
