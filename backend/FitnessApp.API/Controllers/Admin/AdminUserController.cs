using System;
using FitnessApp.Application.Interfaces.Admin;
using FitnessApp.Application.Interfaces.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FitnessApp.API.Controllers.Admin;
/// <summary>
/// Controller for Admin operations on Users (Activate/Deactivate/Reset Password)
/// </summary>
[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Admin")]
public class AdminUserController : ControllerBase
{
    private readonly IUserManagementService _userManagementService;
    private readonly IAdminPasswordResetService _adminPasswordResetService;

    public AdminUserController(
        IUserManagementService userManagementService,
        IAdminPasswordResetService adminPasswordResetService)
    {
        _userManagementService = userManagementService;
        _adminPasswordResetService = adminPasswordResetService;
    }

    /// <summary>
    /// Admin activates a user account
    /// POST /api/admin/users/{userId}/activate
    /// </summary>
    [HttpPost("{userId:int}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ActivateUser([FromRoute] int userId)
    {
        try
        {
            await _userManagementService.ActivateAsync(userId);
            return Ok(new { message = $"User {userId} activated successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Admin deactivates a user account
    /// POST /api/admin/users/{userId}/deactivate
    /// </summary>
    [HttpPost("{userId:int}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeactivateUser([FromRoute] int userId)
    {
        try
        {
            await _userManagementService.DeactivateAsync(userId);
            return Ok(new { message = $"User {userId} deactivated successfully" });
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Admin resets password (generates a new temporary password)
    /// POST /api/admin/users/{userId}/reset-password
    /// </summary>
    [HttpPost("{userId:int}/reset-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ResetPassword([FromRoute] int userId)
    {
        try
        {
            var resp = await _adminPasswordResetService.ResetPasswordAsync(userId);
            return Ok(resp);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Admin gets all users (for management dashboard)
    /// GET /api/admin/users
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllUsers()
    {
        // TODO: Implement IUserQueryService.GetAllAsync()
        return Ok(new { message = "TODO: Implement GetAllUsers" });
    }
}