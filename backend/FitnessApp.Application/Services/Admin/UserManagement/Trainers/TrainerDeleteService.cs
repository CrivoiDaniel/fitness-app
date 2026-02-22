using System;
using FitnessApp.Application.Interfaces.Admin.Trainers;
using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Application.Interfaces.Repositories.Users;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Services.Admin.UserManagement.Trainers;

public class TrainerDeleteService : ITrainerDeleteService
{
    private readonly ITrainerRepository _trainerRepository;
    private readonly IUserRepository _userRepository;

    public TrainerDeleteService(
        ITrainerRepository trainerRepository,
        IUserRepository userRepository)
    {
        _trainerRepository = trainerRepository;
        _userRepository = userRepository;
    }
     public async Task DeleteAsync(int userId)
    {
    // 1. Get user
    var user = await _userRepository.GetByIdAsync(userId);
    
    if (user == null)
        throw new InvalidOperationException($"User with ID {userId} not found");
    
    if (user.Role != Role.Trainer)
        throw new InvalidOperationException($"User with ID {userId} is not a Trainer");

    // 2. Delete User directly
    // EF Core will CASCADE delete the Client automatically
    await _userRepository.DeleteAsync(user);
}   





}
