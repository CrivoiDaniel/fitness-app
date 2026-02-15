using System;
using FitnessApp.Application.DTOs.Admin.Clients;

namespace FitnessApp.Application.Interfaces.Admin.Clients;


/// <summary>
/// Service for querying client data
/// SRP - Responsible ONLY for reading client information
/// </summary>
public interface IClientQueryService
{

    /// <summary>
    /// Gets client by user ID
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>Client details or null if not found</returns>
    Task<ClientDetailsDto?> GetByIdAsync(int userId);

    /// <summary>
    /// Gets all clients
    /// </summary>
    /// <returns>List of all clients</returns>
    Task<List<ClientDetailsDto>> GetAllAsync();

    /// <summary>
    /// Gets active clients only
    /// </summary>
    /// <returns>List of active clients</returns>
    Task<List<ClientDetailsDto>> GetActiveClientsAsync();


    /// <summary>
    /// Checks if client exists by user ID
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>True if exists, false otherwise</returns>
    Task<bool> ExistsAsync(int userId);
}
