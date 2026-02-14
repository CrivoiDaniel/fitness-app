using Microsoft.AspNetCore.Mvc;
using FitnessApp.Application.Interfaces.Users;

namespace FitnessApp.API.Controllers.Admin;

/// <summary>
/// Controller for Admin operations on Users (Activate/Deactivate)
/// RESTful API design - Single Resource (User)
/// SRP - Responsible ONLY for User status management
/// </summary>
[ApiController]
[Route("api/admin/users")]
// TODO: Add [Authorize(Roles = "Admin")] when implementing JWT
public class AdminUserController : ControllerBase
{
    // ========== FIELDS ==========
    private readonly IUserManagementService _userManagementService;

    // ========== CONSTRUCTOR ==========
    public AdminUserController(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }

    // ========== PUBLIC METHODS (User Status Management) ==========

    /// <summary>
    /// Admin activates a user account
    /// POST /api/admin/users/{userId}/activate
    /// </summary>
    /// <param name="userId">User's ID to activate</param>
    /// <returns>Success message</returns>
    [HttpPost("{userId}/activate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ActivateUser(int userId)
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
    /// <param name="userId">User's ID to deactivate</param>
    /// <returns>Success message</returns>
    [HttpPost("{userId}/deactivate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeactivateUser(int userId)
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
    /// Admin gets all users (for management dashboard)
    /// GET /api/admin/users
    /// </summary>
    /// <returns>List of all users</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllUsers()
    {
        // TODO: Implement IUserQueryService.GetAllAsync()
        return Ok(new { message = "TODO: Implement GetAllUsers" });
    }
}