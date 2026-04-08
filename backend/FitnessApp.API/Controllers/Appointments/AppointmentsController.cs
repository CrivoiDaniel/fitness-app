using System;
using System.Collections.Generic;
using System.Linq; // Added for Select
using System.Security.Claims;
using System.Threading.Tasks;
using FitnessApp.Application.DTOs.Appointments; // Added DTO namespace
using FitnessApp.Application.Interfaces.Google;
using FitnessApp.Application.Interfaces.Repositories.Appointments;
using FitnessApp.Application.Interfaces.Repositories.Users;
using FitnessApp.Domain.Entities.Appointments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers.Appointments;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IGoogleCalendarService _googleCalendarService;
    private readonly ITrainerRepository _trainerRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IUserRepository _userRepository;

    public AppointmentsController(
        IAppointmentRepository appointmentRepository,
        IGoogleCalendarService googleCalendarService,
        ITrainerRepository trainerRepository,
        IClientRepository clientRepository,
        IUserRepository userRepository)
    {
        _appointmentRepository = appointmentRepository;
        _googleCalendarService = googleCalendarService;
        _trainerRepository = trainerRepository;
        _clientRepository = clientRepository;
        _userRepository = userRepository;
    }

    [HttpGet("trainer")]
    public async Task<IActionResult> GetTrainerAppointments()
    {
        var userId = GetUserId();
        var trainer = await _trainerRepository.GetByUserIdAsync(userId);
        if (trainer == null) return NotFound("Trainer not found");

        var appointments = await _appointmentRepository.GetTrainerAppointmentsAsync(trainer.Id);
        var dtos = appointments.Select(MapToDto).ToList();
        return Ok(dtos);
    }

    [HttpGet("client")]
    public async Task<IActionResult> GetClientAppointments()
    {
        var userId = GetUserId();
        var client = await _clientRepository.GetByUserIdAsync(userId);
        if (client == null) return NotFound("Client not found");

        var appointments = await _appointmentRepository.GetClientAppointmentsAsync(client.Id);
        var dtos = appointments.Select(MapToDto).ToList();
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentRequest request)
    {
        var userId = GetUserId();
        var trainer = await _trainerRepository.GetByUserIdAsync(userId);
        if (trainer == null) return Unauthorized("Only trainers can create appointments");

        var appointment = new Appointment(
            trainer.Id,
            request.ClientId,
            request.Title,
            request.Description,
            request.StartTime,
            request.EndTime);

        // Sync with Google Calendar if trainer has tokens
        var user = await _userRepository.GetByIdAsync(userId);
        if (user != null && !string.IsNullOrEmpty(user.GoogleRefreshToken))
        {
            var googleEventId = await _googleCalendarService.CreateEventAsync(appointment, user.GoogleRefreshToken);
            appointment.SetGoogleEventId(googleEventId);
        }

        await _appointmentRepository.AddAsync(appointment);

        // Load relations for DTO mapping
        var savedAppointment = await _appointmentRepository.GetByIdAsync(appointment.Id);
        return Ok(MapToDto(savedAppointment!));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentRequest request)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);
        if (appointment == null) return NotFound();

        var userId = GetUserId();
        var trainer = await _trainerRepository.GetByUserIdAsync(userId);
        if (trainer == null || appointment.TrainerId != trainer.Id) 
            return Unauthorized("You can only update your own appointments");

        appointment.Update(request.Title, request.Description, request.StartTime, request.EndTime);

        // Update Google Calendar
        var user = await _userRepository.GetByIdAsync(userId);
        if (user != null && !string.IsNullOrEmpty(user.GoogleRefreshToken) && !string.IsNullOrEmpty(appointment.GoogleEventId))
        {
            await _googleCalendarService.UpdateEventAsync(appointment, user.GoogleRefreshToken);
        }

        await _appointmentRepository.UpdateAsync(appointment);
        return Ok(MapToDto(appointment));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id);
        if (appointment == null) return NotFound();

        var userId = GetUserId();
        var trainer = await _trainerRepository.GetByUserIdAsync(userId);
        if (trainer == null || appointment.TrainerId != trainer.Id) 
            return Unauthorized("You can only delete your own appointments");

        // Delete from Google Calendar
        var user = await _userRepository.GetByIdAsync(userId);
        if (user != null && !string.IsNullOrEmpty(user.GoogleRefreshToken) && !string.IsNullOrEmpty(appointment.GoogleEventId))
        {
            await _googleCalendarService.DeleteEventAsync(appointment.GoogleEventId, user.GoogleRefreshToken);
        }

        await _appointmentRepository.DeleteAsync(appointment);
        return Ok();
    }

    private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

    private AppointmentDto MapToDto(Appointment a)
    {
        return new AppointmentDto
        {
            Id = a.Id,
            Title = a.Title,
            Description = a.Description,
            StartTime = a.StartTime,
            EndTime = a.EndTime,
            ClientId = a.ClientId,
            ClientName = a.Client?.User != null ? $"{a.Client.User.FirstName} {a.Client.User.LastName}" : "Unknown Client",
            TrainerId = a.TrainerId,
            TrainerName = a.Trainer?.User != null ? $"{a.Trainer.User.FirstName} {a.Trainer.User.LastName}" : "Unknown Trainer",
            GoogleEventId = a.GoogleEventId
        };
    }
}

public class CreateAppointmentRequest
{
    public int ClientId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}

public class UpdateAppointmentRequest : CreateAppointmentRequest { }
