using System;

namespace FitnessApp.Application.Interfaces.Admin.Clients;

public interface IClientDeleteService
{
    /// <summary>
    /// Removes both Client and User entities from database
    /// Use only for:
    /// - GDPR "right to be forgotten" compliance
    /// - Data cleanup in testing/development
    /// - Legal requirements
    /// </summary>
    /// <param name="userId">User ID of the client to permanently delete</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when:
    /// - Client not found
    /// - User is not a Client
    /// </exception>
    Task DeleteAsync(int userId);
}
