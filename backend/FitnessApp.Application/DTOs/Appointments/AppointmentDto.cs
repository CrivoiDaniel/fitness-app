using System;

namespace FitnessApp.Application.DTOs.Appointments;

public class AppointmentDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public int TrainerId { get; set; }
    public string TrainerName { get; set; } = string.Empty;
    public string? GoogleEventId { get; set; }
}
