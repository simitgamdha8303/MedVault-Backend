namespace MedVault.Models.Dtos.RequestDtos;

public class DeleteDocumentsRequest
{
    public List<int> DocumentIds { get; set; } = [];
}
