using System;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Factories;

public class TrainerFactory : IUserFactory
{
    // {/* create user for trenner*/}

    public User CreateUser(string email, string password, string firstName, string lastName, Role role)
    {
        if (role != Role.Trainer)
        {
            throw new ArgumentException("Role must be Trainer");
        }
        var passwordHash = HashPassword(password);

        var user = new User(firstName, lastName, email, passwordHash, Role.Trainer);
        return user;
    }
    // {/* create trainer profile for existing user*/}
    public Trainer CreateTrainerProfile(int userId, string specialization, int yearsOfExperience)
    {
        var trainerProfile = new Trainer(userId, specialization, yearsOfExperience);
        return trainerProfile;
    }
    private string HashPassword(string password)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }
}