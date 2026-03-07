using System;
using FitnessApp.Application.Interfaces.Auth;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Factories;

public class TrainerFactory : IUserFactory
{
    private readonly IPasswordHasher _passwordHasher;

    public TrainerFactory(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public User CreateUser(string email, string password, string firstName, string lastName, Role role)
    {
        if (role != Role.Trainer)
            throw new ArgumentException("Role must be Trainer");

        var passwordHash = _passwordHasher.HashPassword(password);
        return new User(firstName, lastName, email, passwordHash, Role.Trainer);
    }

    public Trainer CreateTrainerProfile(int userId, string specialization, int yearsOfExperience)
        => new Trainer(userId, specialization, yearsOfExperience);
}