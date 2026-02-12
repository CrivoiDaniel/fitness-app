using System;
using FitnessApp.Domain.Interfaces;

namespace FitnessApp.Domain.Services;

public class SmsNotificationSender: INotificationSender
{
    public void SendNotification(string recipient, string message)
    {
        // Simulate sending an SMS notification
        Console.WriteLine($"SMS sent to {recipient}: {message}");
    }
}
