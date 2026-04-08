using System;
using FitnessApp.Domain.Entities.Base;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Domain.Entities.Appointments;

public class Appointment : BaseEntity
{
    public int TrainerId { get; private set; }
    public Trainer Trainer { get; private set; } = null!;
    
    public int ClientId { get; private set; }
    public Client Client { get; private set; } = null!;

    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public string? GoogleEventId { get; private set; }

    protected Appointment() : base() { }

    public Appointment(int trainerId, int clientId, string title, string? description, DateTime startTime, DateTime endTime) : base()
    {
        TrainerId = trainerId;
        ClientId = clientId;
        Title = title;
        Description = description;
        StartTime = startTime;
        EndTime = endTime;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string title, string? description, DateTime startTime, DateTime endTime)
    {
        Title = title;
        Description = description;
        StartTime = startTime;
        EndTime = endTime;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetGoogleEventId(string? googleEventId)
    {
        GoogleEventId = googleEventId;
        UpdatedAt = DateTime.UtcNow;
    }
}
