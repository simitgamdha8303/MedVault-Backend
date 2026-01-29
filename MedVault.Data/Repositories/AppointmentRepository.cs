using MedVault.Data.IRepositories;
using MedVault.Models.Entities;

namespace MedVault.Data.Repositories;

public class AppointmentRepository(ApplicationDbContext context) : GenericRepository<Appointment>(context), IAppointmentRepository
{

}