using System;

namespace FitnessApp.Application.Interfaces.Admin.Trainers;

public interface ITrainerDeleteService
{
    Task DeleteAsync(int userId);
}
