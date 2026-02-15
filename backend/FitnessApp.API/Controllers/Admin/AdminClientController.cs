using System;
using FitnessApp.Application.DTOs.Admin.Clients;
using FitnessApp.Application.DTOs.Admin;
using FitnessApp.Application.Interfaces.Admin.Clients;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers.Admin;

/// <summary>
/// Controller for Admin operations on Clients
/// </summary>
[ApiController]
[Route("api/admin/clients")]
// TODO: Add [Authorize(Roles = "Admin")] when implementing JWT
public class AdminClientController : ControllerBase
{
    private readonly IClientCreationService _clientCreationService;
    private readonly IClientQueryService _clientQueryService;
    private readonly IClientUpdateService _clientUpdateService;
    private readonly IClientDeleteService _clientDeletionService;


    public AdminClientController(IClientCreationService clientCreationService, IClientQueryService clientQueryService,  IClientUpdateService clientUpdateService,
        IClientDeleteService clientDeletionService)
    {
        _clientCreationService = clientCreationService;
        _clientQueryService = clientQueryService;
        _clientUpdateService = clientUpdateService;
        _clientDeletionService = clientDeletionService;
    }

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
                nameof(GetClient),           
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
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClientDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ClientDetailsDto>> GetClient([FromRoute] int id)
    {
        var client = await _clientQueryService.GetByIdAsync(id);
        
        if (client == null)
            return NotFound(new { message = $"Client with user ID {id} not found" });
        
        return Ok(client);
    }

    /// <summary>
    /// Admin gets all Clients
    /// GET /api/admin/clients
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<ClientDetailsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClientDetailsDto>>> GetAllClients()
    {
        var clients = await _clientQueryService.GetAllAsync();
        return Ok(clients);
    }

    /// <summary>
    /// Admin gets active Clients only
    /// GET /api/admin/clients/active
    /// </summary>
    [HttpGet("active")]
    [ProducesResponseType(typeof(List<ClientDetailsDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClientDetailsDto>>> GetActiveClients()
    {
        var clients = await _clientQueryService.GetActiveClientsAsync();
        return Ok(clients);
    }

    /// <summary>
    /// Admin updates Client profile
    /// PUT /api/admin/clients/{id}
    /// Admin can modify: email, name, phone, status, all fitness data
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ClientDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ClientDetailsDto>> UpdateClient(
        [FromRoute] int id,
        [FromBody] UpdateClientDto dto)
    {
        try
        {
            var updatedClient = await _clientUpdateService.UpdateAsync(id, dto);
            return Ok(updatedClient);
        }
        catch (InvalidOperationException ex)
        {
            // Client not found OR Email already in use
            if (ex.Message.Contains("not found"))
                return NotFound(new { message = ex.Message });
            else
                return Conflict(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /// <summary>
    /// Admin permanently deletes Client (hard delete - DANGEROUS!)
    /// DELETE /api/admin/clients/{id}/permanent
    /// Use only for GDPR compliance or data cleanup
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> DeleteClient([FromRoute] int id)
    {
        try
        {
            await _clientDeletionService.DeleteAsync(id);
            return Ok(new 
            { 
                message = $"Client with user ID {id} has been permanently deleted",
                warning = "This action cannot be undone!"
            });
        }
        catch (InvalidOperationException e)
        {
            if (e.Message.Contains("not found"))
                return NotFound(new { message = e.Message });
            else
                return BadRequest(new { message = e.Message });
        }
    }
 
}