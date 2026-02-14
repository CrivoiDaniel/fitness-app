using System;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.Interfaces.Repositories;

public interface IClientRepository
{
    Task<Client?> GetByIdAsync(int id);
    Task<Client?> GetByUserIdAsync(int userId);
    Task<Client> AddAsync(Client client);
    Task UpdateAsync(Client client);
}