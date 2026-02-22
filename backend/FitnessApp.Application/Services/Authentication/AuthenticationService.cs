using System;
using System.Threading.Tasks;
using FitnessApp.Application.DTOs.Authentication;
using FitnessApp.Application.Interfaces.Authentication;
using FitnessApp.Application.Interfaces.Repositories;
using FitnessApp.Application.Interfaces.Repositories.Users;

namespace FitnessApp.Application.Services.Authentication;

/// <summary>
/// Service for user authentication
/// Responsible ONLY for login/logout/token validation
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly IUserRepository _userRepository;
    public AuthenticationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Authenticates a user with email and password
    /// </summary>
    /// <param name="loginDto">Login credentials</param>
    /// <returns>Login response with user info and token</returns>
    /// <exception cref="UnauthorizedAccessException">If credentials are invalid or account is deactivated</exception>
    public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
    {
        // 1. Find user by email
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        
        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password");

        // 2. Verify password (simplified - production: BCrypt.Verify)
        if (!VerifyPassword(loginDto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password");

        // 3. Check if account is active
        if (!user.IsActive)
            throw new UnauthorizedAccessException("Account is deactivated");

        // 4. Generate token (demo for now - later: JWT)
        var token = GenerateToken(user.Id, user.Role.ToString());

        // 5. Return response
        return new LoginResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role.ToString(),
            Token = token,
            Message = "Login successful"
        };
    }

    /// <summary>
    /// Validates a JWT token
    /// </summary>
    /// <param name="token">JWT token to validate</param>
    /// <returns>True if token is valid, false otherwise</returns>
    public async Task<bool> ValidateTokenAsync(string token)
    {
        // TODO: Implement JWT validation
        // For now, returns true for demo purposes
        return await Task.FromResult(!string.IsNullOrEmpty(token));
    }

    //PRIVATE HELPER METHODS
    
    /// <summary>
    /// Verifies if password matches the stored hash
    /// </summary>
    /// <param name="password">Plain text password</param>
    /// <param name="passwordHash">Stored password hash</param>
    /// <returns>True if password is correct</returns>
    private bool VerifyPassword(string password, string passwordHash)
    {
        var hash = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes(password)
        );
        return hash == passwordHash;
    }

    /// <summary>
    /// Generates JWT token for authenticated user
    /// </summary>
    /// <param name="userId">User's ID</param>
    /// <param name="role">User's role</param>
    /// <returns>JWT token (demo for now)</returns>
    private string GenerateToken(int userId, string role)
    {
        // TODO: Implement real JWT generation
        // For now, returns demo token
        return $"demo-token-{userId}-{role}";
    }
}