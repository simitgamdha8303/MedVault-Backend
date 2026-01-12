namespace MedVault.Models.Dtos.RequestDtos;

public class DocumentRequest
{
    public int MedicalTimelineId { get; set; }
    public string FileName { get; set; } = null!;
    public string FileUrl { get; set; } = null!;
    public DateOnly DocumentDate { get; set; }
}

