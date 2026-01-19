using MedVault.Data.IRepositories;
using MedVault.Models.Entities;

namespace MedVault.Data.Repositories;

public class HospitalRepository(ApplicationDbContext context) : GenericRepository<Hospital>(context), IHospitalRepository
{

}