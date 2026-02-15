using System;

namespace FitnessApp.Application.DTOs.Admin.Trainers;

public class TrainerDetailsDto
{
    //user info
    public int UserId { get; set;}
    public string Email {get; set;} = string.Empty;
    public string FirstName {get; set;} = string.Empty;
    public string LastName {get; set;} = string.Empty;
    public string? PhoneNumber {get; set;}
    public string Role {get; set;} = string.Empty;
    public bool IsActive {get; set;} 

    //trainer specific info
    public int TrainerId {get; set;}

    public string Specialization {get; set;} = string.Empty;
    public int YearsOfExperience {get; set;}

    public DateTime CreatedAt {get; set;}
    public DateTime UpdatedAt {get; set;}

}
