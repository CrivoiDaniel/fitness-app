using System;
using FitnessApp.Application.Interfaces.Auth;
using FitnessApp.Application.Interfaces.Repositories.Users;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers.Admin;

[ApiController]
[Route("api/admin/dev")]
public class DevAdminController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IWebHostEnvironment _env;

    public DevAdminController(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IWebHostEnvironment env)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _env = env;
    }

    public record ResetPasswordRequest(string Email, string NewPassword);

    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct)
    {
        if (!_env.IsDevelopment())
            return NotFound(); // ascunde endpoint-ul în prod

        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest(new { message = "Email is required" });

        if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 6)
            return BadRequest(new { message = "NewPassword must be at least 6 characters" });

        var user = await _userRepository.GetByEmailAsync(request.Email, ct);
        if (user == null)
            return NotFound(new { message = $"User with email {request.Email} not found" });

        var newHash = _passwordHasher.HashPassword(request.NewPassword);
        user.SetPasswordHash(newHash);

        await _userRepository.UpdateAsync(user, ct);

        return Ok(new { message = "Password reset successfully", email = user.Email });
    }
}