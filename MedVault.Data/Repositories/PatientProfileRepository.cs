using MedVault.Data.IRepositories;
using MedVault.Models.Entities;

namespace MedVault.Data.Repositories;

public class PatientProfileRepository : GenericRepository<PatientProfile>, IPatientProfileRepository
{
    public PatientProfileRepository(ApplicationDbContext context)
        : base(context)
    {
    }
}
