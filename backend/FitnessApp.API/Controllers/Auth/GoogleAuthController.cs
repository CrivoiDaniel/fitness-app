using System.Security.Claims;
using FitnessApp.Application.Interfaces.Google;
using FitnessApp.Application.Interfaces.Repositories.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GoogleAuthController : ControllerBase
{
    private readonly IGoogleCalendarService _googleCalendarService;
    private readonly IUserRepository _userRepository;

    public GoogleAuthController(IGoogleCalendarService googleCalendarService, IUserRepository userRepository)
    {
        _googleCalendarService = googleCalendarService;
        _userRepository = userRepository;
    }

    [HttpGet("url")]
    public IActionResult GetAuthUrl()
    {
        var url = _googleCalendarService.GetAuthUrl();
        return Ok(new { url });
    }

    [HttpPost("callback")]
    public async Task<IActionResult> Callback([FromBody] GoogleAuthRequest request)
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdString, out var userId)) 
            return Unauthorized();

        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) 
            return NotFound();

        try 
        {
            var (refreshToken, email) = await _googleCalendarService.GetTokensFromCodeAsync(request.Code);
            
            // Note: RefreshToken might be null on subsequent logins if not using prompt=consent.
            // But for the FIRST connection, it's required.
            
            user.UpdateGoogleTokens(refreshToken, email);
            await _userRepository.UpdateAsync(user);

            return Ok(new { message = "Google Calendar connected successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = "Google Auth failed", details = ex.Message });
        }
    }
}

public class GoogleAuthRequest
{
    public string Code { get; set; } = string.Empty;
}
