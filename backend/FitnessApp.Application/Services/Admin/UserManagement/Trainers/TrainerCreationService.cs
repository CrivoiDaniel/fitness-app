using System;
using System.Linq;
using System.Threading.Tasks;
using FitnessApp.Application.DTOs.Admin.Trainers;
using FitnessApp.Application.DTOs.Admin;
using FitnessApp.Application.Factories;
using FitnessApp.Application.Interfaces.Admin.Trainers;
using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Domain.Enums;
using FitnessApp.Application.Interfaces.Repositories.Users;

namespace FitnessApp.Application.Services.Admin.UserManagement.Trainers;

/// <summary>
/// Service for creating trainers by Admin
/// Responsible ONLY for trainer creation
/// </summary>
public class TrainerCreationService : ITrainerCreationService
{
    private const int MinPasswordLength = 8;
    private static readonly char[] AllowedSymbols = { '!', '@', '#', '$', '%', '&', '*' };

    private readonly IUserRepository _userRepository;
    private readonly ITrainerRepository _trainerRepository;
    private readonly TrainerFactory _trainerFactory;

    public TrainerCreationService(
        IUserRepository userRepository,
        ITrainerRepository trainerRepository,
        TrainerFactory trainerFactory)
    {
        _userRepository = userRepository;
        _trainerRepository = trainerRepository;
        _trainerFactory = trainerFactory;
    }

    /// <summary>
    /// Admin creates account for Trainer
    /// Factory Method Pattern in action!
    /// </summary>
    /// <param name="dto">Trainer creation data transfer object</param>
    /// <returns>Response with user info and temporary password</returns>
    /// <exception cref="InvalidOperationException">If email already exists</exception>
    public async Task<CreateUserResponseDto> CreateAsync(CreateTrainerDto dto)
    {
        // 1. Validate business rules
        await ValidateEmailNotExistsAsync(dto.Email);

        // 2. Generate temporary password (minimum 8 characters)
        var temporaryPassword = GenerateTemporaryPassword(dto.LastName);

        // 3. Create User using TrainerFactory (Factory Method Pattern)
        var user = _trainerFactory.CreateUser(
            dto.Email,
            temporaryPassword,
            dto.FirstName,
            dto.LastName,
            Role.Trainer
        );

        // 4. Set phone number (optional)
        if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
            user.SetPhoneNumber(dto.PhoneNumber);

        // 5. Save User to database
        await _userRepository.AddAsync(user);

        // 6. Create Trainer profile using TrainerFactory
        var trainer = _trainerFactory.CreateTrainerProfile(
            user.Id,
            dto.Specialization,
            dto.YearsOfExperience
        );

        // 7. Save Trainer profile to database
        await _trainerRepository.AddAsync(trainer);

        // 8. TODO: Send welcome email with temporary password
        // await _emailService.SendWelcomeEmailAsync(user.Email, temporaryPassword);

        // 9. Return response
        return new CreateUserResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString(),
            TemporaryPassword = temporaryPassword,
            Message = "Trainer created successfully. Temporary password generated."
        };
    }

    /// <summary>
    /// Validates that email does not already exist in the system
    /// </summary>
    /// <param name="email">Email to validate</param>
    /// <exception cref="InvalidOperationException">If email already exists</exception>
    private async Task ValidateEmailNotExistsAsync(string email)
    {
        if (await _userRepository.ExistsAsync(email))
            throw new InvalidOperationException($"Email '{email}' already exists in the system.");
    }

    /// <summary>
    /// Generates personalized temporary password (minimum 8 characters)
    /// Format: LastName + (Symbol + Digit)* until minimum 8 characters
    /// </summary>
    /// <param name="lastName">User's last name</param>
    /// <returns>Temporary password (e.g., "Smith!7$9")</returns>
    private string GenerateTemporaryPassword(string lastName)
    {
        var random = new Random();

        // Capitalize first letter from lastName
        var lastNameCapitalized = CapitalizeFirstLetter(lastName);

        // Shuffle symbols for randomization
        var shuffledSymbols = AllowedSymbols.OrderBy(x => random.Next()).ToArray();
        int symbolIndex = 0;

        // Start with lastName
        var password = lastNameCapitalized;

        // Add pairs (Symbol + Digit) until minimum length
        while (password.Length < MinPasswordLength)
        {
            char symbol = shuffledSymbols[symbolIndex % shuffledSymbols.Length];
            symbolIndex++;

            int digit = random.Next(0, 10);

            password += symbol;
            password += digit;
        }

        return password;
    }

    /// <summary>
    /// Capitalizes first letter of string and converts rest to lowercase
    /// </summary>
    /// <param name="text">Text to capitalize</param>
    /// <returns>Text with first letter capitalized</returns>
    private string CapitalizeFirstLetter(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        text = text.Trim().ToLower();
        return char.ToUpper(text[0]) + text.Substring(1);
    }
}