using FitnessApp.Domain.Entities.Appointments;
using FitnessApp.Application.Interfaces.Repositories;

namespace FitnessApp.Application.Interfaces.Repositories.Appointments;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<List<Appointment>> GetTrainerAppointmentsAsync(int trainerId);
    Task<List<Appointment>> GetClientAppointmentsAsync(int clientId);
}
