using System;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.Interfaces.Repositories.Users;

public interface ITrainerRepository
{
    Task<Trainer?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<List<Trainer>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Trainer> AddAsync(Trainer trainer, CancellationToken cancellationToken = default);
    Task UpdateAsync(Trainer trainer, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    Task<Trainer?> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<List<Trainer>> GetActiveTrainersAsync(CancellationToken cancellationToken = default);
    Task<bool> ExistsByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}