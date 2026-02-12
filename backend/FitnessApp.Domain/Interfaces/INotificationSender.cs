using System;

namespace FitnessApp.Domain.Interfaces;

public interface INotificationSender
{
    void SendNotification(string recipient, string message);

}
