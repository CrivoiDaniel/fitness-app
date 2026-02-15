using System;
using System.Linq;
using System.Threading.Tasks;
using FitnessApp.Application.DTOs.Admin.Clients;
using FitnessApp.Application.DTOs.Admin;
using FitnessApp.Application.Factories;
using FitnessApp.Application.Interfaces.Admin.Clients;
using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Domain.Enums;

namespace FitnessApp.Application.Services.Admin.UserManagement.Clients;

/// <summary>
/// Service for creating clients by Admin
/// Responsible ONLY for client creation
/// </summary>
public class ClientCreationService : IClientCreationService
{
    private const int MinPasswordLength = 8;
    private static readonly char[] AllowedSymbols = { '!', '@', '#', '$', '%', '&', '*' };

    private readonly IUserRepository _userRepository;
    private readonly IClientRepository _clientRepository;
    private readonly ClientFactory _clientFactory;

    public ClientCreationService(
        IUserRepository userRepository,
        IClientRepository clientRepository,
        ClientFactory clientFactory)
    {
        _userRepository = userRepository;
        _clientRepository = clientRepository;
        _clientFactory = clientFactory;
    }

    /// <summary>
    /// Admin creates account for Client
    /// Factory Method Pattern in action!
    /// </summary>
    /// <param name="dto">Client creation data transfer object</param>
    /// <returns>Response with user info and temporary password</returns>
    /// <exception cref="InvalidOperationException">If email already exists</exception>
    public async Task<CreateUserResponseDto> CreateAsync(CreateClientDto dto)
    {
        // 1. Validate business rules
        await ValidateEmailNotExistsAsync(dto.Email);

        // 2. Generate temporary password (minimum 8 characters)
        var temporaryPassword = GenerateTemporaryPassword(dto.LastName);

        // 3. Create User using ClientFactory (Factory Method Pattern)
        var user = _clientFactory.CreateUser(
            dto.Email,
            temporaryPassword,
            dto.FirstName,
            dto.LastName,
            Role.Client
        );

        // 4. Set phone number (optional)
        if (!string.IsNullOrWhiteSpace(dto.PhoneNumber))
            user.SetPhoneNumber(dto.PhoneNumber);

        // 5. Save User to database
        await _userRepository.AddAsync(user);

        // 6. Create Client profile using ClientFactory
        var client = _clientFactory.CreateClientProfile(user.Id, dto.DateOfBirth);

        // 7. Save Client profile to database
        await _clientRepository.AddAsync(client);

        // 8. TODO: Send email with temporary password
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
            Message = "Client created successfully. Temporary password generated."
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
    /// <example>
    /// Examples:
    ///   - "Do" → "Do#5@3!8" (8 characters)
    ///   - "Smith" → "Smith!7$9" (10 characters)
    /// </example>
    private string GenerateTemporaryPassword(string lastName)
    {
        var random = new Random();

        // 1. Capitalize first letter from lastName
        var lastNameCapitalized = CapitalizeFirstLetter(lastName);

        // 2. Shuffle symbols for randomization and uniqueness
        var shuffledSymbols = AllowedSymbols.OrderBy(x => random.Next()).ToArray();
        int symbolIndex = 0;

        // 3. Start with lastName
        var password = lastNameCapitalized;

        // 4. Add pairs (Symbol + Digit) until reaching minimum 8 characters
        while (password.Length < MinPasswordLength)
        {
            // Take next symbol from shuffled array (wrap around if needed)
            char symbol = shuffledSymbols[symbolIndex % shuffledSymbols.Length];
            symbolIndex++;

            // Generate random digit (0-9)
            int digit = random.Next(0, 10);

            // Append symbol and digit
            password += symbol;
            password += digit;
        }

        return password;
    }

    /// <summary>
    /// Capitalizes first letter of string and converts rest to lowercase
    /// </summary>
    /// <param name="text">Text to capitalize</param>
    /// <returns>Text with first letter capitalized (e.g., "doe" → "Doe")</returns>
    private string CapitalizeFirstLetter(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        // Trim and lowercase entire string
        text = text.Trim().ToLower();

        // Capitalize first letter
        return char.ToUpper(text[0]) + text.Substring(1);
    }
}
