using MedVault.Data.IRepositories;
using MedVault.Models.Entities;

namespace MedVault.Data.Repositories;

public class HospitalRepository : GenericRepository<Hospital>, IHospitalRepository
{
    public HospitalRepository(ApplicationDbContext context) : base(context)
    {
    }

}