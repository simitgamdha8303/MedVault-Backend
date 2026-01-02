using MedVault.Data.IRepositories;
using MedVault.Models.Entities;

namespace MedVault.Data.Repositories;

public class OtpRepository : GenericRepository<OtpVerification>, IOtpRepository
{
    public OtpRepository(ApplicationDbContext context) : base(context)
    {
    }

}
