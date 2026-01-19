namespace MedVault.Data.Repositories;
using MedVault.Data.IRepositories;
using MedVault.Models.Entities;

public class UserRoleRepository(ApplicationDbContext context) : GenericRepository<UserRole>(context), IUserRoleRepository
{
}
