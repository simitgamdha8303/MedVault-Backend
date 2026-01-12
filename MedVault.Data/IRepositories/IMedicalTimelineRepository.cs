using System.Linq.Expressions;
using MedVault.Models.Entities;

namespace MedVault.Data.IRepositories;

public interface IMedicalTimelineRepository : IGenericRepository<MedicalTimeline>
{
    Task<List<TResult>> GetListAsync<TResult>(Expression<Func<MedicalTimeline, bool>> predicate,Expression<Func<MedicalTimeline, TResult>> selector);
}