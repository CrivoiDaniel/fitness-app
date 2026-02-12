using System;
using FitnessApp.Domain.Entities.Base;
using FitnessApp.Domain.Enums;
namespace FitnessApp.Domain.Entities.Users;

public class Trainer : BaseEntity
{
    public int UserId { get; private set; }
    public string Specialization { get; private set; } = string.Empty;
    public int YearsOfExperience { get; private set; }
    public decimal Rating { get; private set; }


    // {/* property for user relation*/}
    public User user { get; set; } = null!;
    public Trainer() : base()
    {
        Rating = 5.0m; // Default rating for new trainers
    }
    public Trainer(int userId, string specialization, int yearsOfExperience) : base()
    {
        UserId = userId;
        SetSpecialization(specialization);
        SetExperience(yearsOfExperience);
        Rating = 5.0m; // Default rating for new trainers
    }
    //{/* Validation Input  */}
    public void SetSpecialization(string specialization)
    {
        if(string.IsNullOrWhiteSpace(specialization))
        {
            throw new ArgumentException("Specialization cannot be empty.");
        }
        Specialization = specialization.Trim();
        UpdateTimestamp();
    }
    public void SetExperience(int years)
    {
        if(years < 0)
        {
            throw new ArgumentException("Years of experience cannot be negative.");
        }
        YearsOfExperience = years;
        UpdateTimestamp();
    }
    public void UpdateRating(decimal rating)
    {
        if(rating < 0 || rating > 5)
        {
            throw new ArgumentException("Rating must be between 0 and 5.");
        }
        Rating = rating;
        UpdateTimestamp();
    }
}

