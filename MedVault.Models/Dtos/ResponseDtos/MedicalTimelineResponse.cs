namespace MedVault.Models.Dtos.ResponseDtos;

using MedVault.Models.Enums;

public class MedicalTimelineResponse
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public string? DoctorProfileName { get; set; }
    public int? DoctorProfileId { get; set; }

    public string? DoctorName { get; set; }

    public string CheckupType { get; set; } = default!;

    public CheckupType CheckupTypeId { get; set; }

    public DateTime EventDate { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public List<DocumentResponse>? DocumentResponses { get; set; }
}
