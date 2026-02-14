using System;
using FitnessApp.Application.DTOs.Admin;

namespace FitnessApp.Application.Interfaces.Admin;

public interface IClientCreationService
{
    Task<CreateUserResponseDto> CreateAsync(CreateClientDto dto);
}
