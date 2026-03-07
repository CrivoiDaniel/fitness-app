using System;
using FitnessApp.Application.DTOs.Admin;
using FitnessApp.Application.Interfaces.Admin;
using FitnessApp.Application.Interfaces.Auth;
using FitnessApp.Application.Interfaces.Repositories.Users;

namespace FitnessApp.Application.Services.Users;

public class AdminPasswordResetService : IAdminPasswordResetService
{
    private const int MinPasswordLength = 8;
    private static readonly char[] AllowedSymbols = { '!', '@', '#', '$', '%', '&', '*' };

    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public AdminPasswordResetService(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ResetPasswordResponseDto> ResetPasswordAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException($"User with ID {userId} not found");

        var tempPassword = GenerateTemporaryPassword(user.LastName);

        user.SetPasswordHash(_passwordHasher.HashPassword(tempPassword));
        user.RequirePasswordChange();

        await _userRepository.UpdateAsync(user);

        return new ResetPasswordResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            TemporaryPassword = tempPassword,
            Message = "Temporary password generated. User must change password at next login."
        };
    }

    private string GenerateTemporaryPassword(string seed)
    {
        var random = new Random();
        var basePart = string.IsNullOrWhiteSpace(seed) ? "User" : char.ToUpper(seed.Trim()[0]) + seed.Trim().ToLower().Substring(1);

        var password = basePart;
        int symbolIndex = 0;
        var symbols = AllowedSymbols.OrderBy(_ => random.Next()).ToArray();

        while (password.Length < MinPasswordLength)
        {
            password += symbols[symbolIndex % symbols.Length];
            password += random.Next(0, 10);
            symbolIndex++;
        }

        return password;
    }
}