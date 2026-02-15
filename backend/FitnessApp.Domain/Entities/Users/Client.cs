using System;
using FitnessApp.Domain.Entities.Base;
using FitnessApp.Domain.Enums;
namespace FitnessApp.Domain.Entities.Users;

public class Client : BaseEntity
{
    public int UserId { get; private set; }
    public DateTime DateOfBirth { get; private set; }


    //{/* property for user relation*/}
    public User user { get; set; } = null!;
    protected Client() : base() { }
    public Client(int userId, DateTime dateOfBirth) : base()
    {
        UserId = userId;
        SetDateOfBirth(dateOfBirth);
    }

    // {/* Validation Input  */}
    public void SetDateOfBirth(DateTime dateOfBirth)
    {
        var age = DateTime.UtcNow.Year - dateOfBirth.Year;

        if (age < 15)
            throw new ArgumentException("Client must be at least 15 years old");

        if (age > 120)
            throw new ArgumentException("Invalid date of birth");
        DateOfBirth = dateOfBirth;
        UpdateTimestamp();
    }
}
