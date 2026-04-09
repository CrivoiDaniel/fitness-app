using System.Threading.Tasks;
using FitnessApp.Domain.Entities.Subscriptions;
using FitnessApp.Domain.Observer;
using FitnessApp.Domain.Interfaces;

namespace FitnessApp.Application.Observer;

/// <summary>
/// Observer Pattern: Concrete Observer
/// Trimiterea unui email de bun venit / confirmare după plată.
/// </summary>
public class EmailPurchaseObserver : ISubscriptionObserver
{
    private readonly INotificationSender _emailSender;

    public EmailPurchaseObserver(INotificationSender emailSender)
    {
        _emailSender = emailSender;
    }

    public Task OnPurchaseCompletedAsync(Subscription subscription, decimal amount)
    {
        string message = $@"
            <h3>Felicitări pentru achiziție!</h3>
            <p>Abonamentul tău a fost activat cu succes.</p>
            <ul>
                <li>Total plătit: <b>{amount} MDL</b></li>
                <li>Dată început: {subscription.StartDate:dd/MM/yyyy}</li>
            </ul>
            <p>Spor la antrenamente!</p>";

        // În acest proiect, Client.User.Email ar trebui să fie disponibil dacă repository-ul a făcut Include.
        // Pentru acest lab, folosim un email generic dacă lipsește.
        string email = "client@fitnessapp.com"; 
        
        _emailSender.SendNotification(email, message);
        
        return Task.CompletedTask;
    }
}
