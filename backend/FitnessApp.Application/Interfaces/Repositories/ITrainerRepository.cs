using System;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.Interfaces.Repositories;

public interface ITrainerRepository
{
    Task<Trainer?> GetByIdAsync(int id);
    Task<Trainer?> GetByUserIdAsync(int userId);
    Task<Trainer> AddAsync(Trainer trainer);    
    Task UpdateAsync(Trainer trainer);

}
