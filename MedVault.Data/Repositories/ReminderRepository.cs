using MedVault.Data.IRepositories;
using MedVault.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedVault.Data.Repositories;

public class ReminderRepository(ApplicationDbContext context) : GenericRepository<Reminder>(context), IReminderRepository
{

    public async Task<Reminder?> GetReminderWithPatientAsync(int reminderId)
    {
        return await context.Set<Reminder>()
            .Include(r => r.PatientProfile)
            .FirstOrDefaultAsync(r => r.Id == reminderId);
    }
}