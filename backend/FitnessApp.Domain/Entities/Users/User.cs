using System;
using FitnessApp.Domain.Entities.Base;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Domain.Entities.Users;

public class User : BaseEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string? PhoneNumber { get; private set; }
    public Role Role { get; private set; }
    public bool IsActive { get; private set; }

    public Client? ClientProfile { get; set; }
    public Trainer? TrainerProfile { get; set; }

    protected User()
    {
        IsActive = true;
        Role = Role.Client; // Default role
    }
    public User(string firstName, string lastName, string email, string passwordHash, Role role)
    {
        SetName(firstName, lastName);
        SetEmail(email);
        SetPasswordHash(passwordHash);
        SetPhoneNumber(null);
        Role = role;
        IsActive = true;
    }
    //{/* Validation Input*/}

    public void SetName(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("First name and last name cannot be empty.");
        }
        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        UpdateTimestamp();
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty.");
        }
        Email = email.Trim().ToLower();
        UpdateTimestamp();
    }
    public void SetPasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new ArgumentException("Password hash cannot be null or empty.");
        }
        PasswordHash = passwordHash;
        UpdateTimestamp();
    }
    public void SetPhoneNumber(string? phoneNumber)
    {
        if (phoneNumber is null)
        {
            PhoneNumber = null;
            return;
        }

        PhoneNumber = phoneNumber.Trim();
        UpdateTimestamp();
    }

    public void Activate()
    {
        IsActive = true;
        UpdateTimestamp();
    }
    public void Deactivate()
    {
        IsActive = false;
        UpdateTimestamp();
    }
    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }
}
