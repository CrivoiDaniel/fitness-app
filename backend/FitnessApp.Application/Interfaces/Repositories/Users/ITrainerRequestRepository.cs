using System.Collections.Generic;
using System.Threading.Tasks;
using FitnessApp.Domain.Entities.Users;

namespace FitnessApp.Application.Interfaces.Repositories.Users
{
    public interface ITrainerRequestRepository : IRepository<TrainerRequest>
    {
        Task<List<TrainerRequest>> GetPendingByTrainerUserIdAsync(int trainerUserId);
    }
}
