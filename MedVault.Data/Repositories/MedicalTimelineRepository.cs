using MedVault.Data.IRepositories;
using MedVault.Models.Entities;

namespace MedVault.Data.Repositories;

public class MedicalTimelineRepository
 : GenericRepository<MedicalTimeline>, IMedicalTimelineRepository
{
    public MedicalTimelineRepository(ApplicationDbContext context) : base(context)
    {
    }

}