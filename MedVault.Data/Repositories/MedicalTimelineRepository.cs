using System.Linq.Expressions;
using MedVault.Data.IRepositories;
using MedVault.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace MedVault.Data.Repositories;

public class MedicalTimelineRepository
 : GenericRepository<MedicalTimeline>, IMedicalTimelineRepository
{
    public MedicalTimelineRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<TResult>> GetListAsync<TResult>(Expression<Func<MedicalTimeline, bool>> predicate,Expression<Func<MedicalTimeline, TResult>> selector)
    {
        return await _context.MedicalTimelines
            .Include(x => x.DoctorProfile)
                .ThenInclude(d => d.User)
            .Where(predicate)
            .Select(selector)
            .ToListAsync();
    }

}