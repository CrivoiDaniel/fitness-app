using System;
using FitnessApp.Application.DTOs.Admin.Clients;
using FitnessApp.Application.DTOs.Admin.Trainers;

namespace FitnessApp.Application.Interfaces.Admin.Trainers;

public interface ITrainerUpdateService
{
    Task<TrainerDetailsDto> UpdateAsync(int userId, UpdateTrainerDto dto);

}
