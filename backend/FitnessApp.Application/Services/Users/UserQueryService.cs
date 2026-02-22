using System;
using System.Threading.Tasks;
using FitnessApp.Application.DTOs.Users;
using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Application.Interfaces.Repositories.Users;
using FitnessApp.Application.Interfaces.Users;

namespace FitnessApp.Application.Services.Users;

/// <summary>
/// Service for user query operations (read-only)
/// </summary>
public class UserQueryService : IUserQueryService
{
    private readonly IUserRepository _userRepository;
    public UserQueryService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    /// <summary>
    /// Gets user by ID
    /// </summary>
    /// <param name="id">User's ID</param>
    /// <returns>User DTO or null if not found</returns>
    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        
        if (user == null)
            return null;

        return MapToDto(user);
    }

    /// <summary>
    /// Gets user by email
    /// </summary>
    /// <param name="email">User's email</param>
    /// <returns>User DTO or null if not found</returns>
    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        
        if (user == null)
            return null;

        return MapToDto(user);
    }

    /// <summary>
    /// Checks if email already exists in the system
    /// </summary>
    /// <param name="email">Email to check</param>
    /// <returns>True if email exists, false otherwise</returns>
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _userRepository.ExistsAsync(email);
    }

    //PRIVATE HELPER METHODS

    /// <summary>
    /// Maps User entity to UserDto
    /// Removes sensitive data (passwords, etc.)
    /// </summary>
    /// <param name="user">User entity</param>
    /// <returns>User DTO without sensitive information</returns>
    private UserDto MapToDto(Domain.Entities.Users.User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role.ToString(),
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt
        };
    }

}