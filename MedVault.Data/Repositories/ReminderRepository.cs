using MedVault.Data.IRepositories;
using MedVault.Models.Entities;

namespace MedVault.Data.Repositories;

public class ReminderRepository(ApplicationDbContext context) : GenericRepository<Reminder>(context), IReminderRepository
{

}