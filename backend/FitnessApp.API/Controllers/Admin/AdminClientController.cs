using System;
using FitnessApp.Application.DTOs.Admin;
using FitnessApp.Application.Interfaces.Admin;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers.Admin;

/// <summary>
/// Controller for Admin operations on Clients
/// RESTful API design - Single Resource (Client)
/// SRP - Responsible ONLY for Client CRUD operations
/// </summary>
[ApiController]
[Route("api/admin/clients")]
// TODO: Add [Authorize(Roles = "Admin")] when implementing JWT
public class AdminClientController : ControllerBase
{
    // ========== FIELDS ==========
    private readonly IClientCreationService _clientCreationService;
    // TODO: Add IClientUpdateService, IClientDeletionService, IClientQueryService

    // ========== CONSTRUCTOR ==========
    public AdminClientController(IClientCreationService clientCreationService)
    {
        _clientCreationService = clientCreationService;
    }

    // ========== PUBLIC METHODS (CRUD) ==========

    /// <summary>
    /// Admin creates a new Client
    /// POST /api/admin/clients
    /// </summary>
    /// <param name="dto">Client creation data</param>
    /// <returns>Created client with temporary password</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateUserResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateUserResponseDto>> CreateClient(
        [FromBody] CreateClientDto dto)
    {
        try
        {
            var response = await _clientCreationService.CreateAsync(dto);
            return CreatedAtAction(
                nameof(GetClient),           // ✅ Acum metoda există
                new { id = response.UserId },
                response
            );
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Admin gets Client by ID
    /// GET /api/admin/clients/{id}
    /// </summary>
    /// <param name="id">Client's user ID</param>
    /// <returns>Client details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetClient(int id)  // ← Verifică că parametrul e "int id"
    {
        // TODO: Implement IClientQueryService.GetByIdAsync()
        return Ok(new { message = $"TODO: Implement GetClient for ID {id}" });
    }

    /// <summary>
    /// Admin gets all Clients
    /// GET /api/admin/clients
    /// </summary>
    /// <returns>List of all clients</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllClients()
    {
        // TODO: Implement IClientQueryService.GetAllAsync()
        return Ok(new { message = "TODO: Implement GetAllClients" });
    }

    /// <summary>
    /// Admin updates Client profile
    /// PUT /api/admin/clients/{id}
    /// </summary>
    /// <param name="id">Client's user ID</param>
    /// <param name="dto">Updated client data</param>
    /// <returns>Updated client</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateClient(int id, [FromBody] object dto)
    {
        // TODO: Implement IClientUpdateService.UpdateAsync()
        return Ok(new { message = $"TODO: Implement UpdateClient for ID {id}" });
    }

    /// <summary>
    /// Admin deletes Client (soft delete)
    /// DELETE /api/admin/clients/{id}
    /// </summary>
    /// <param name="id">Client's user ID</param>
    /// <returns>Success message</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteClient(int id)
    {
        // TODO: Implement IClientDeletionService.DeleteAsync()
        return Ok(new { message = $"TODO: Implement DeleteClient for ID {id}" });
    }
}