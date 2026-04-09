using System.Net;
using System.Net.Mail;
using FitnessApp.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace FitnessApp.Infrastructure.Services.Observer;

/// <summary>
/// Infrastructure Layer Service: Real SMTP Implementation
/// Uses System.Net.Mail to send actual emails based on appsettings.json configuration.
/// </summary>
public class SmtpEmailNotificationSender : INotificationSender
{
    private readonly IConfiguration _configuration;

    public SmtpEmailNotificationSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendNotification(string recipient, string message)
    {
        try
        {
            var host = _configuration["SmtpSettings:Host"];
            var port = int.Parse(_configuration["SmtpSettings:Port"] ?? "587");
            var username = _configuration["SmtpSettings:Username"];
            var password = _configuration["SmtpSettings:Password"];
            var enableSsl = bool.Parse(_configuration["SmtpSettings:EnableSsl"] ?? "true");
            var fromEmail = _configuration["SmtpSettings:FromEmail"];
            var fromName = _configuration["SmtpSettings:FromName"];

            if (string.IsNullOrEmpty(username) || username == "your-email@gmail.com")
            {
                Console.WriteLine("[SMTP MOCK] No real credentials provided. Email to {0}: {1}", recipient, message);
                return;
            }

            using var smtpClient = new SmtpClient(host)
            {
                Port = port,
                Credentials = new NetworkCredential(username, password),
                EnableSsl = enableSsl,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail!, fromName),
                Subject = "Fitness App Notification",
                Body = message,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(recipient);

            smtpClient.Send(mailMessage);
            Console.WriteLine("[SMTP SUCCESS] Real email sent to {0}", recipient);
        }
        catch (Exception ex)
        {
            Console.WriteLine("[SMTP ERROR] Failed to send email to {0}: {1}", recipient, ex.Message);
            // In a real production app, we would use a more robust logger and possibly a background queue.
        }
    }
}
