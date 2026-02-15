using System;
using System.Data;

namespace FitnessApp.Application.DTOs.Admin.Clients;


/// <summary>
/// DTO for detailed client information
/// </summary>
public class ClientDetailsDto
{

    //user info
    public int UserId { get; set;}
    public string Email {get; set;} = string.Empty;
    public string FirstName {get; set;} = string.Empty;
    public string LastName {get; set;} = string.Empty;
    public string? PhoneNumber {get; set;}
    public string Role {get; set;} = string.Empty;
    public bool IsActive {get; set;} 

    //client specific info
    public int ClientId {get; set;}
    public DateTime DateOfBirth {get; set;}

    public int Age => DateTime.UtcNow.Year - DateOfBirth.Year;

    public DateTime CreatedAt {get; set;}
    public DateTime UpdatedAt {get; set;}

}
