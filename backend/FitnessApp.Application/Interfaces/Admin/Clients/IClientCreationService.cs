using System;
using FitnessApp.Application.DTOs.Admin.Clients;

namespace FitnessApp.Application.Interfaces.Admin.Clients;
using FitnessApp.Application.DTOs.Admin;
public interface IClientCreationService
{
    Task<CreateUserResponseDto> CreateAsync(CreateClientDto dto);
}
