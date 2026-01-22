using System.ComponentModel.DataAnnotations;

namespace MedVault.Models.Dtos.RequestDtos;

public class DocumentRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int MedicalTimelineId { get; set; }

    [Required]
    public string FileName { get; set; } = null!;

    [Required]
    [Url]
    public string FileUrl { get; set; } = null!;

    [Required]
    public DateTime DocumentDate { get; set; }
}

