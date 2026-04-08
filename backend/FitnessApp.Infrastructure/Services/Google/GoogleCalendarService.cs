using FitnessApp.Application.Interfaces.Google;
using FitnessApp.Domain.Entities.Appointments;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace FitnessApp.Infrastructure.Services.Google;

public class GoogleCalendarService : IGoogleCalendarService
{
    private readonly IConfiguration _configuration;
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _redirectUri;

    public GoogleCalendarService(IConfiguration configuration)
    {
        _configuration = configuration;
        _clientId = _configuration["GoogleSettings:ClientId"]!;
        _clientSecret = _configuration["GoogleSettings:ClientSecret"]!;
        _redirectUri = _configuration["GoogleSettings:RedirectUri"]!;
    }

    private CalendarService GetCalendarService(string refreshToken)
    {
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _clientId,
                ClientSecret = _clientSecret
            }
        });

        var tokenResponse = new TokenResponse { RefreshToken = refreshToken };
        var credential = new UserCredential(flow, "user", tokenResponse);

        return new CalendarService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "FitnessApp"
        });
    }

    public async Task<string?> CreateEventAsync(Appointment appointment, string refreshToken)
    {
        try 
        {
            var service = GetCalendarService(refreshToken);

            var ev = new Event
            {
                Summary = appointment.Title,
                Description = appointment.Description,
                Start = new EventDateTime { DateTimeRaw = appointment.StartTime.ToString("yyyy-MM-ddTHH:mm:ssZ") },
                End = new EventDateTime { DateTimeRaw = appointment.EndTime.ToString("yyyy-MM-ddTHH:mm:ssZ") },
            };

            var request = service.Events.Insert(ev, "primary");
            var createdEvent = await request.ExecuteAsync();

            return createdEvent.Id;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> UpdateEventAsync(Appointment appointment, string refreshToken)
    {
        if (string.IsNullOrEmpty(appointment.GoogleEventId)) return false;

        try 
        {
            var service = GetCalendarService(refreshToken);

            var ev = new Event
            {
                Summary = appointment.Title,
                Description = appointment.Description,
                Start = new EventDateTime { DateTimeRaw = appointment.StartTime.ToString("yyyy-MM-ddTHH:mm:ssZ") },
                End = new EventDateTime { DateTimeRaw = appointment.EndTime.ToString("yyyy-MM-ddTHH:mm:ssZ") }
            };

            var request = service.Events.Update(ev, "primary", appointment.GoogleEventId);
            await request.ExecuteAsync();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteEventAsync(string googleEventId, string refreshToken)
    {
        if (string.IsNullOrEmpty(googleEventId)) return false;

        try 
        {
            var service = GetCalendarService(refreshToken);
            await service.Events.Delete("primary", googleEventId).ExecuteAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public string GetAuthUrl()
    {
        var scopes = new[] { 
            CalendarService.Scope.CalendarEvents,
            "https://www.googleapis.com/auth/userinfo.email",
            "openid"
        };

        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _clientId,
                ClientSecret = _clientSecret
            },
            Scopes = scopes
        });

        var authUrl = flow.CreateAuthorizationCodeRequest(_redirectUri).Build();
        return authUrl.ToString() + "&access_type=offline&prompt=consent";
    }

    public async Task<(string RefreshToken, string Email)> GetTokensFromCodeAsync(string code)
    {
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = _clientId,
                ClientSecret = _clientSecret
            },
            Scopes = new[] { 
                CalendarService.Scope.CalendarEvents,
                "https://www.googleapis.com/auth/userinfo.email",
                "openid"
            }
        });

        var tokenResponse = await flow.ExchangeCodeForTokenAsync("user", code, _redirectUri, CancellationToken.None);
        
        // Note: RefreshToken might be null if not requesting 'offline' or if already granted.
        // But GetAuthUrl adds access_type=offline.
        
        return (tokenResponse.RefreshToken ?? "", ""); 
    }
}
