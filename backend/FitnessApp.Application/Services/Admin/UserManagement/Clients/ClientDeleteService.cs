using System;
using FitnessApp.Application.Interfaces.Admin.Clients;
using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Services.Admin.UserManagement.Clients;

public class ClientDeleteService : IClientDeleteService
{
    private readonly IClientRepository _clientRepository;
    private readonly IUserRepository _userRepository;

    public ClientDeleteService(
        IClientRepository clientRepository,
        IUserRepository userRepository)
    {
        _clientRepository = clientRepository;
        _userRepository = userRepository;
    }
    /// <summary>
    /// delete - permanently removes client and user from database
    /// </summary>
    public async Task DeleteAsync(int userId)
    {
    // 1. Get user
    var user = await _userRepository.GetByIdAsync(userId);
    
    if (user == null)
        throw new InvalidOperationException($"User with ID {userId} not found");
    
    if (user.Role != Role.Client)
        throw new InvalidOperationException($"User with ID {userId} is not a Client");

    // 2. Delete User directly
    // EF Core will CASCADE delete the Client automatically
    await _userRepository.DeleteAsync(user);
    
    // Done! Both User and Client are deleted
    }   

}
