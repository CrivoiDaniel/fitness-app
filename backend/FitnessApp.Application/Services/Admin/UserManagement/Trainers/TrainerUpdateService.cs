using System;
using FitnessApp.Application.DTOs.Admin.Clients;
using FitnessApp.Application.DTOs.Admin.Trainers;
using FitnessApp.Application.Interfaces.Admin.Trainers;
using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.Services.Admin.UserManagement.Trainers;

public class TrainerUpdateService : ITrainerUpdateService
{
    private readonly ITrainerRepository _trainerRepository;
    private readonly IUserRepository _userRepository;
    private readonly ITrainerQueryService _trainerQueryService;

    public TrainerUpdateService(
        ITrainerRepository trainerRepository,
        IUserRepository userRepository,
        ITrainerQueryService trainerQueryService
    )
    {
        _trainerRepository = trainerRepository;
        _userRepository = userRepository;
        _trainerQueryService = trainerQueryService;
    }

    public async Task<TrainerDetailsDto> UpdateAsync(int userId, UpdateTrainerDto dto)
    {
        // get clinet by user id
        var trainer = await _trainerRepository.GetByUserIdAsync(userId);

        if (trainer == null)
            throw new InvalidOperationException($"Trainer with user Id {userId} not found");

        bool userUpdated = false;

        // email change
        if(!string.IsNullOrWhiteSpace(dto.Email) &&
            dto.Email.ToLower().Trim() != trainer.user.Email.ToLower())
        {
            var normalizedEmail = dto.Email.ToLower().Trim();

            var existingUser = await _userRepository.GetByEmailAsync(normalizedEmail);
            if(existingUser != null && existingUser.Id != userId)
                throw new InvalidOperationException($"Email '{dto.Email}' is already in use");

            trainer.user.ChangeEmail(normalizedEmail);
            userUpdated = true;
        }

        // First Name
        if (!string.IsNullOrWhiteSpace(dto.FirstName) && 
            dto.FirstName.Trim() != trainer.user.FirstName)
        {
            trainer.user.UpdateFirstName(dto.FirstName);
            userUpdated = true;
        }
        
        // Last Name
        if (!string.IsNullOrWhiteSpace(dto.LastName) && 
            dto.LastName.Trim() != trainer.user.LastName)
        {
            trainer.user.UpdateLastName(dto.LastName);
            userUpdated = true;
        }
        
        // Phone Number (can be set to null to clear)
        if (dto.PhoneNumber != trainer.user.PhoneNumber)
        {
            trainer.user.SetPhoneNumber(dto.PhoneNumber);
            userUpdated = true;
        }
        
        // Account Status (Admin privilege)
        if (dto.IsActive.HasValue && dto.IsActive.Value != trainer.user.IsActive)
        {
            if (dto.IsActive.Value)
                trainer.user.Activate();
            else
                trainer.user.Deactivate();
            
            userUpdated = true;
        }

        // UPDATE trainer ENTITY
        
        bool trainerUpdated = false;

        if(!string.IsNullOrWhiteSpace(dto.Specialization) &&
            dto.Specialization.Trim() != trainer.Specialization)
        {
            trainer.SetSpecialization(dto.Specialization);
            trainerUpdated = true;
        }

        if (dto.YearsOfExperience.HasValue && 
            dto.YearsOfExperience.Value != trainer.YearsOfExperience)
        {
            trainer.SetYearsOfExperience(dto.YearsOfExperience.Value);
            trainerUpdated = true;
        }

        if(userUpdated)
            await _userRepository.UpdateAsync(trainer.user);
        
        if(trainerUpdated)
            await _trainerRepository.UpdateAsync(trainer);

        return (await _trainerQueryService.GetByIdAsync(userId))!;
    }

}
