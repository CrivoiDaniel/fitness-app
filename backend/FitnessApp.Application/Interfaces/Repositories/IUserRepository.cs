using System;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.Interfaces.Repositories;
public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<User> AddAsync(User user); 
    Task UpdateAsync(User user);
    Task<bool> ExistsAsync(string email);

}
