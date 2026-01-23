using MedVault.Data.IRepositories;
using MedVault.Models.Entities;

namespace MedVault.Data.Repositories;

public class QrShareRepository(ApplicationDbContext context) : GenericRepository<QrShare>(context), IQrShareRepository
{

}
