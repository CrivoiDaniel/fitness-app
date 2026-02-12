using System;
using FitnessApp.Domain.Interfaces;

namespace FitnessApp.Domain.Services;

public class EmailNotificationSender : INotificationSender
{
    public void SendNotification(string recipient, string message)
    {
        // Simulate sending an email notification
        Console.WriteLine($"Email sent to {recipient}: {message}");
    }

}
