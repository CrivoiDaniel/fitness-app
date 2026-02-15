using System;
using System.Security.Cryptography.X509Certificates;
using FitnessApp.Application.DTOs.Admin.Clients;
using FitnessApp.Application.Interfaces.Admin.Clients;
using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.Services.Admin.UserManagement.Clients;


/// <summary>
/// Service for querying client data
/// Maps entities to DTOs
/// </summary>
public class ClientQueryService : IClientQueryService
{
    private readonly IClientRepository _clientRepository;

    public ClientQueryService(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    
    /// <summary>
    /// Gets client by user ID
    /// </summary>
    public async Task<ClientDetailsDto?> GetByIdAsync(int userId)
    {
        var client = await _clientRepository.GetByUserIdAsync(userId);
        
        if (client == null)
            return null;
        
        return MapToDetailsDto(client);
    }

    /// <summary>
    /// Gets all clients
    /// </summary>
    public async Task<List<ClientDetailsDto>> GetAllAsync()
    {
        var clients = await _clientRepository.GetAllAsync();
        
        return clients.Select(MapToDetailsDto).ToList();
    }

    /// <summary>
    /// Gets active clients only
    /// </summary>
    public async Task<List<ClientDetailsDto>> GetActiveClientsAsync()
    {
        var clients = await _clientRepository.GetActiveClientsAsync();
        
        return clients.Select(MapToDetailsDto).ToList();
    }

    /// <summary>
    /// Checks if client exists
    /// </summary>
    public async Task<bool> ExistsAsync(int userId)
    {
        return await _clientRepository.ExistsByUserIdAsync(userId);
    }

    //PRIVATE HELPER METHODS

    /// <summary>
    /// Maps Client entity to ClientDetailsDto
    /// </summary>
    private ClientDetailsDto MapToDetailsDto(Client client)
    {
        return new ClientDetailsDto
        {
            // User info
            UserId = client.user.Id,
            Email = client.user.Email,
            FirstName = client.user.FirstName,
            LastName = client.user.LastName,
            PhoneNumber = client.user.PhoneNumber,
            Role = client.user.Role.ToString(),
            IsActive = client.user.IsActive,
            
            // Client info
            ClientId = client.Id,
            DateOfBirth = client.DateOfBirth,
            
            // Audit info
            CreatedAt = client.CreatedAt,
            UpdatedAt = client.UpdatedAt
        };
    }

}
