using FitnessApp.Application.Interfaces.Repositories.Appointments;
using FitnessApp.Domain.Entities.Appointments;
using FitnessApp.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessApp.Infrastructure.Repositories.Appointments;

public class AppointmentRepository : Repository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ApplicationDbContext context) : base(context) { }

    public override async Task<Appointment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(a => a.Client)
                .ThenInclude(c => c.User)
            .Include(a => a.Trainer)
                .ThenInclude(t => t.User)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<List<Appointment>> GetTrainerAppointmentsAsync(int trainerId)
    {
        return await _dbSet
            .Include(a => a.Client)
            .ThenInclude(c => c.User)
            .Where(a => a.TrainerId == trainerId)
            .ToListAsync();
    }

    public async Task<List<Appointment>> GetClientAppointmentsAsync(int clientId)
    {
        return await _dbSet
            .Include(a => a.Trainer)
            .ThenInclude(t => t.User)
            .Where(a => a.ClientId == clientId)
            .ToListAsync();
    }
}
