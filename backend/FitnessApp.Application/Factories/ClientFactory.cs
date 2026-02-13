using System;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Factories;

public class ClientFactory : IUserFactory
{
    public User CreateUser(string email, string password, string firstName, string lastName, Role role)
    {
        if (role != Role.Client)
        {
            throw new ArgumentException("Role must be Client");
        }
        var passwordHash = HashPassword(password);

        var user = new User(firstName, lastName, email, passwordHash, Role.Client);
        return user;
    }

    // {/* create client profile for existing user*/}
    public Client CreateClientProfile(int userId, DateTime dateOfBirth)
    {
        var clientProfile = new Client(userId, dateOfBirth);
        return clientProfile;
    }

    private string HashPassword(string password)
    {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }
}
