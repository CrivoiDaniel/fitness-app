using System;
using FitnessApp.Application.DTOs.Admin.Clients;

namespace FitnessApp.Application.Interfaces.Admin.Clients;

public interface IClientUpdateService
{
    /// <summary>
    /// Updates client profile
    /// Admin can update: email, name, phone, status, all fitness data
    /// </summary>
    /// <param name="userId">User ID of the client to update</param>
    /// <param name="dto">Updated client data</param>
    /// <returns>Updated client details</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when:
    /// - Client not found
    /// - Email already in use by another user
    /// </exception>
    /// <exception cref="ArgumentException">
    /// </exception>
    
    Task<ClientDetailsDto> UpdateAsync(int userId, UpdateClientDto dto);
}
