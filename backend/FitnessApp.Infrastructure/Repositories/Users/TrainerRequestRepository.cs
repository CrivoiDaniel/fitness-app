using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessApp.Application.Interfaces.Repositories.Users;
using FitnessApp.Domain.Entities.Users;
using FitnessApp.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Infrastructure.Repositories.Users
{
    public class TrainerRequestRepository : Repository<TrainerRequest>, ITrainerRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public TrainerRequestRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<TrainerRequest>> GetPendingByTrainerUserIdAsync(int trainerUserId)
        {
            var trainer = await _context.Trainers.FirstOrDefaultAsync(t => t.UserId == trainerUserId);
            if (trainer == null) return new List<TrainerRequest>();

            return await _context.TrainerRequests
                .Include(r => r.Client)
                .ThenInclude(c => c.User)
                .Where(r => r.TrainerId == trainer.Id && r.Status == "Pending")
                .ToListAsync();
        }
    }
}
