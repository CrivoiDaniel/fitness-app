using System;
using FitnessApp.Application.DTOs.Users;

namespace FitnessApp.Application.Interfaces.Users;

public interface IUserQueryService
{
    // {/* User Queries */}
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);

}