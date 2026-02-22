using System;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.Interfaces.Repositories.Users;

/// <summary>
/// Repository interface for Client entity
/// </summary>
public interface IClientRepository
{
    Task<Client?> GetByIdAsync(int id);
    Task<Client?> GetByUserIdAsync(int userId);
    Task<List<Client>> GetAllAsync();
    Task<List<Client>> GetActiveClientsAsync();
    Task<bool> ExistsByUserIdAsync(int userId);
    Task<Client> AddAsync(Client client);
    Task UpdateAsync(Client client);
    Task DeleteAsync(Client client);
}
