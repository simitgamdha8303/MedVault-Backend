using MedVault.Data.IRepositories;
using MedVault.Models.Entities;

namespace MedVault.Data.Repositories;

public class DoctorProfileRepository : GenericRepository<DoctorProfile>, IDoctorProfileRepository
{
    public DoctorProfileRepository(ApplicationDbContext context) : base(context)
    {
    }

}