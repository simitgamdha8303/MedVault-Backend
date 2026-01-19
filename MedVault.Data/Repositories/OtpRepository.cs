using MedVault.Data.IRepositories;
using MedVault.Models.Entities;

namespace MedVault.Data.Repositories;

public class OtpRepository(ApplicationDbContext context) : GenericRepository<OtpVerification>(context), IOtpRepository
{

}
