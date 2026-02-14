using System;
using System.Threading.Tasks;
using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Application.Interfaces.Users;

namespace FitnessApp.Application.Services.Users;

/// <summary>
/// Service for user management operations (activate/deactivate)
/// </summary>
public class UserManagementService : IUserManagementService
{
    private readonly IUserRepository _userRepository;
    public UserManagementService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Activates a user account
    /// </summary>
    /// <param name="userId">ID of the user to activate</param>
    /// <exception cref="InvalidOperationException">If user not found</exception>

    public async Task ActivateAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        
        if (user == null)
            throw new InvalidOperationException($"User with ID {userId} not found.");

        user.Activate();
        await _userRepository.UpdateAsync(user);
    }

    /// <summary>
    /// Deactivates a user account
    /// </summary>
    /// <param name="userId">ID of the user to deactivate</param>
    /// <exception cref="InvalidOperationException">If user not found</exception>
    public async Task DeactivateAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        
        if (user == null)
            throw new InvalidOperationException($"User with ID {userId} not found.");

        user.Deactivate();
        await _userRepository.UpdateAsync(user);
    }
}