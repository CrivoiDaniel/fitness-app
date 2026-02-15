using System;
using FitnessApp.Application.DTOs.Admin.Trainers;
using FitnessApp.Application.DTOs.Admin;

namespace FitnessApp.Application.Interfaces.Admin.Trainers;

public interface ITrainerCreationService
{
    Task<CreateUserResponseDto> CreateAsync(CreateTrainerDto dto);

}
