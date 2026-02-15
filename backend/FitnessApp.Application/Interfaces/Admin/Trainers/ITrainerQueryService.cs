using System;
using FitnessApp.Application.DTOs.Admin.Trainers;

namespace FitnessApp.Application.Interfaces.Admin.Trainers;

public interface ITrainerQueryService
{


    /// <summary>
    /// Gets trainers by user ID
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>trainers details or null if not found</returns>
    Task<TrainerDetailsDto?> GetByIdAsync(int userId);

    /// <summary>
    /// Gets all trainers
    /// </summary>
    /// <returns>List of all trainers</returns>
    Task<List<TrainerDetailsDto>> GetAllAsync();


    /// <summary>
    /// Gets active trainers only
    /// </summary>
    /// <returns>List of active trainers</returns>
    Task<List<TrainerDetailsDto>> GetActiveTrainersAsync();

    /// <summary>
    /// Checks if trainers exists by user ID
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>True if exists, false otherwise</returns>
    Task<bool> ExistsAsync(int userId);





}
