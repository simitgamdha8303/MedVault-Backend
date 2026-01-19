using MedVault.Data.IRepositories;
using MedVault.Models.Entities;

namespace MedVault.Data.Repositories;

public class PatientProfileRepository(ApplicationDbContext context) : GenericRepository<PatientProfile>(context), IPatientProfileRepository
{
}
