using System;
using FitnessApp.Application.DTOs.Admin.Clients;
using FitnessApp.Application.Interfaces.Admin.Clients;
using FitnessApp.Application.Interfaces.Repositories;

namespace FitnessApp.Application.Services.Admin.UserManagement.Clients;

public class ClientUpdateService : IClientUpdateService
{
    private readonly IClientRepository _clientRepository;
    private readonly IUserRepository _userRepository;
    private readonly IClientQueryService _clientQueryService;

    public ClientUpdateService(
        IClientRepository clientRepository,
        IUserRepository userRepository,
        IClientQueryService clientQueryService)
    {
        _clientRepository = clientRepository;
        _userRepository = userRepository;
        _clientQueryService = clientQueryService;
    }

    public async Task<ClientDetailsDto> UpdateAsync(int userId, UpdateClientDto dto)
    {
        // 1. Get client by user ID
        var client = await _clientRepository.GetByUserIdAsync(userId);
        
        if (client == null)
            throw new InvalidOperationException($"Client with user ID {userId} not found");

        //UPDATE USER ENTITY 
        bool userUpdated = false;

        // Email change (check uniqueness first)
        if (!string.IsNullOrWhiteSpace(dto.Email) && 
            dto.Email.ToLower().Trim() != client.user.Email.ToLower())
        {
            var normalizedEmail = dto.Email.ToLower().Trim();
            
            // Check if email already exists
            var existingUser = await _userRepository.GetByEmailAsync(normalizedEmail);
            if (existingUser != null && existingUser.Id != userId)
                throw new InvalidOperationException($"Email '{dto.Email}' is already in use");
            
            client.user.ChangeEmail(normalizedEmail);
            userUpdated = true;
        }
        
        // First Name
        if (!string.IsNullOrWhiteSpace(dto.FirstName) && 
            dto.FirstName.Trim() != client.user.FirstName)
        {
            client.user.UpdateFirstName(dto.FirstName);
            userUpdated = true;
        }
        
        // Last Name
        if (!string.IsNullOrWhiteSpace(dto.LastName) && 
            dto.LastName.Trim() != client.user.LastName)
        {
            client.user.UpdateLastName(dto.LastName);
            userUpdated = true;
        }
        
        // Phone Number (can be set to null to clear)
        if (dto.PhoneNumber != client.user.PhoneNumber)
        {
            client.user.SetPhoneNumber(dto.PhoneNumber);
            userUpdated = true;
        }
        
        // Account Status (Admin privilege)
        if (dto.IsActive.HasValue && dto.IsActive.Value != client.user.IsActive)
        {
            if (dto.IsActive.Value)
                client.user.Activate();
            else
                client.user.Deactivate();
            
            userUpdated = true;
        }

        // UPDATE CLIENT ENTITY
        
        bool clientUpdated = false;
        
        // Date of Birth
        if (dto.DateOfBirth.HasValue && dto.DateOfBirth.Value.Date != client.DateOfBirth.Date)
        {
            client.SetDateOfBirth(dto.DateOfBirth.Value);
            clientUpdated = true;
        }
        
        // ========== SAVE CHANGES ==========
        
        if (userUpdated)
            await _userRepository.UpdateAsync(client.user);
        
        if (clientUpdated)
            await _clientRepository.UpdateAsync(client);

        // Return updated client
        return (await _clientQueryService.GetByIdAsync(userId))!;
    }



}
