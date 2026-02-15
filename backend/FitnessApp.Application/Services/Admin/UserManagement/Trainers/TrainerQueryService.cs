using System;
using FitnessApp.Application.DTOs.Admin.Trainers;
using FitnessApp.Application.Interfaces.Admin.Trainers;
using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.Services.Admin.UserManagement.Trainers;

public class TrainerQueryService : ITrainerQueryService
{
    private readonly ITrainerRepository _trainerRepository;

    public TrainerQueryService(ITrainerRepository trainerRepository)
    {
        _trainerRepository = trainerRepository;
    }

    public async Task<TrainerDetailsDto?> GetByIdAsync(int userId)
    {
        var trainer = await _trainerRepository.GetByUserIdAsync(userId);

        if (trainer == null)
        {
            return null;
        }
        return MapToDetailsDto(trainer);
    }

    public async Task<List<TrainerDetailsDto>> GetAllAsync()
    {
        var trainers = await _trainerRepository.GetAllAsync();

        return trainers.Select(MapToDetailsDto).ToList();
    }

    public async  Task<List<TrainerDetailsDto>> GetActiveTrainersAsync()
    {
        var trainers = await _trainerRepository.GetActiveTrainersAsync();
        return trainers.Select(MapToDetailsDto).ToList();
    }

    public async Task<bool> ExistsAsync(int userId)
    {
        return await _trainerRepository.ExistsByUserIdAsync(userId);
    }

    // private helper methods
    private TrainerDetailsDto MapToDetailsDto(Trainer Trainer)
    {
        return new TrainerDetailsDto
        {
            UserId = Trainer.user.Id,
            Email = Trainer.user.Email,
            FirstName = Trainer.user.FirstName,
            LastName = Trainer.user.LastName,
            PhoneNumber = Trainer.user.PhoneNumber,
            Role = Trainer.user.Role.ToString(),
            IsActive = Trainer.user.IsActive,

            TrainerId = Trainer.Id,
            Specialization = Trainer.Specialization,
            YearsOfExperience = Trainer.YearsOfExperience,

            // Audit info
            CreatedAt = Trainer.CreatedAt,
            UpdatedAt = Trainer.UpdatedAt
        };
    }

}
