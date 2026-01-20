using MedVault.Data.IRepositories;
using MedVault.Models.Entities;

namespace MedVault.Data.Repositories;

public class ReminderTypeRepository(ApplicationDbContext context) : GenericRepository<ReminderType>(context), IReminderTypeRepository
{

}