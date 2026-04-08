using FitnessApp.Domain.Entities.Appointments;

namespace FitnessApp.Application.Interfaces.Google;

public interface IGoogleCalendarService
{
    Task<string?> CreateEventAsync(Appointment appointment, string refreshToken);
    Task<bool> UpdateEventAsync(Appointment appointment, string refreshToken);
    Task<bool> DeleteEventAsync(string googleEventId, string refreshToken);
    string GetAuthUrl();
    Task<(string RefreshToken, string Email)> GetTokensFromCodeAsync(string code);
}
