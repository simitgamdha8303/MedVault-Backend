using MedVault.Data.IRepositories;
using MedVault.Models.Entities;

namespace MedVault.Data.Repositories;

public class DocumentRepository(ApplicationDbContext context) : GenericRepository<Document>(context), IDocumentRepository
{

}