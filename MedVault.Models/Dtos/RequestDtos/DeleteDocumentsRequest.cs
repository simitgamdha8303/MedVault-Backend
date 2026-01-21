using System.ComponentModel.DataAnnotations;
using MedVault.Common.Messages;

namespace MedVault.Models.Dtos.RequestDtos;

public class DeleteDocumentsRequest
{
    [Required]
    [MinLength(1, ErrorMessage = ValidationMessages.AT_LEAST_ONE_DOCUMENT_REQUIRED)]
    public List<int> DocumentIds { get; set; } = [];
}
