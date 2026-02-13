using System;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Factories;

public interface IUserFactory
{
    //{/* create base user */}
    User CreateUser(string email, string passwordHash, string firstName, string lastName, Role role);

}
