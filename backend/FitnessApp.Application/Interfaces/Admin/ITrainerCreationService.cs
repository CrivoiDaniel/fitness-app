using System;
using FitnessApp.Application.DTOs.Admin;

namespace FitnessApp.Application.Interfaces.Admin;

public interface ITrainerCreationService
{
    Task<CreateUserResponseDto> CreateAsync(CreateTrainerDto dto);

}
