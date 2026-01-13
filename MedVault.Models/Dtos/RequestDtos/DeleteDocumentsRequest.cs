using System.ComponentModel.DataAnnotations;

namespace MedVault.Models.Dtos.RequestDtos;

public class DeleteDocumentsRequest
{
    [Required]
    [MinLength(1, ErrorMessage = "At least one document id is required")]
    public List<int> DocumentIds { get; set; } = [];
}
