namespace MedVault.Data.Repositories;
using MedVault.Data.IRepositories;
using MedVault.Models.Entities;

public class UserRoleRepository : GenericRepository<UserRole>, IUserRoleRepository
{
     public UserRoleRepository(ApplicationDbContext context) : base(context)
    {
    }
}
