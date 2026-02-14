using System;

namespace FitnessApp.Application.Interfaces.Users;

public interface IUserManagementService
{
    Task ActivateAsync(int userId);
    Task DeactivateAsync(int userId);
}
