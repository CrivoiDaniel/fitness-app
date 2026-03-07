using System;
using FitnessApp.Application.Interfaces.Auth;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Factories;

public class ClientFactory : IUserFactory
{
    private readonly IPasswordHasher _passwordHasher;

    public ClientFactory(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public User CreateUser(string email, string password, string firstName, string lastName, Role role)
    {
        if (role != Role.Client)
            throw new ArgumentException("Role must be Client");

        var passwordHash = _passwordHasher.HashPassword(password);
        return new User(firstName, lastName, email, passwordHash, Role.Client);
    }

    public Client CreateClientProfile(int userId, DateTime dateOfBirth)
        => new Client(userId, dateOfBirth);
}